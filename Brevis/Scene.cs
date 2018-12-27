using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Brevis
{
    internal class Scene : IDrawable
    {
        private readonly WriteableBitmap _bitmap;
        private readonly int _color;
        internal Scene(WriteableBitmap bitmap, int color)
        {
            this._bitmap = bitmap;
            this._color = color;
        }

        internal void StartDrawing()
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

        internal void EndDrawing()
        {
            this._bitmap.AddDirtyRect(new System.Windows.Int32Rect(0, 0, this._bitmap.PixelWidth, this._bitmap.PixelHeight));
            this._bitmap.Unlock();
        }

        internal void SetPixel(int row, int column, int color)
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

        internal int GetPixel(int row, int column)
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
    }
}
