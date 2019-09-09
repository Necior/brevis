using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Brevis
{
    public class Scene : IHasSetPixel
    {
        private readonly WriteableBitmap _bitmap;
        private readonly int _color;
        private double[,] _zbuffer;
        private List<Tuple<int, int, double, int, bool>> pixelsQueue; /* TODO: change structure to keep `z` per [`x`, `y`] for more efficient sorting. */
        public Scene(WriteableBitmap bitmap, int color)
        {
            this._bitmap = bitmap;
            this._color = color;
            _zbuffer = new double[_bitmap.PixelHeight, _bitmap.PixelWidth];
            this.pixelsQueue = new List<Tuple<int, int, double, int, bool>>();
            ResetZbuffer();
        }

        public void ResetZbuffer()
        {
            for (int row = 0; row < _bitmap.PixelHeight; row++)
            for (int column = 0; column < _bitmap.PixelWidth; column++)
                _zbuffer[row, column] = double.MaxValue;
        }

        public void StartDrawing()
        {
            this._bitmap.Lock();
            this.Clear();
        }

        private void Clear()
        {
            unsafe
            {
                for (var row = 0; row < this._bitmap.PixelHeight; row++)
                {
                    for (var column = 0; column < this._bitmap.PixelWidth; column++)
                    {
                        var pBackBuffer = (int)_bitmap.BackBuffer;
                        pBackBuffer += row * _bitmap.BackBufferStride;
                        pBackBuffer += column * 4;
                        *((int*)pBackBuffer) = this._color;
                    }
                }
            }
        }

        public void EndDrawing(bool transparent)
        {
            if (transparent)
            {
                Clear();
                pixelsQueue.Sort((t1, t2) => t2.Item3.CompareTo(t1.Item3));
                foreach (var p in pixelsQueue)
                {
                    if (p.Item5)
                    {
                        var oldColor = GetPixel(p.Item1, p.Item2);
                        var newColor = p.Item4;
                        var mixedColor = (((((oldColor & 0xff0000) >> 16) + ((newColor & 0xff0000) >> 16)) / 2) << 16) +
                                         (((((oldColor & 0x00ff00) >> 8) + ((newColor & 0x00ff00) >> 8)) / 2) << 8) +
                                         (((oldColor & 0x0000ff) + (newColor & 0x0000ff)) / 2);
                        SetPixel(p.Item1, p.Item2, mixedColor);
                    }
                    else
                    {
                        SetPixel(p.Item1, p.Item2, p.Item4);
                    }
                }
            }
            this._bitmap.AddDirtyRect(new System.Windows.Int32Rect(0, 0, this._bitmap.PixelWidth, this._bitmap.PixelHeight));
            this._bitmap.Unlock();
            this.pixelsQueue.Clear();
        }

        public void SetPixel(int row, int column, int color)
        {
            if(this.IsOutside(row, column))
                return;
            unsafe
            {
                var pBackBuffer = (int)_bitmap.BackBuffer;
                pBackBuffer += row * _bitmap.BackBufferStride;
                pBackBuffer += column * 4;
                *((int*)pBackBuffer) = color;
            }
        }

        public int GetPixel(int row, int column)
        {
            if(this.IsOutside(row, column))
                return 0; // assume black color outside of scene
            unsafe
            {
                var pBackBuffer = (int)_bitmap.BackBuffer;
                pBackBuffer += row * _bitmap.BackBufferStride;
                pBackBuffer += column * 4;
                return *((int*)pBackBuffer);
            }
        }

        private bool IsInside(int row, int column)
        {
            return row >= 0 && column >= 0 && row < this._bitmap.PixelHeight && column < this._bitmap.PixelWidth;
        }

        private bool IsOutside(int row, int column)
        {
            return !this.IsInside(row, column);
        }

        public void SetPixel(int row, int column, double z, int color)
        {
            if (_zbuffer[row, column] > z)
            {
                _zbuffer[row, column] = z;
                SetPixel(row, column, color);
            }
        }

        public void SetPixel(int row, int column, double z, int color, bool transparent)
        {
            this.pixelsQueue.Add(new Tuple<int, int, double, int, bool>(row, column, z, color, transparent));
            SetPixel(row, column, z, color);
        }
    }
}
