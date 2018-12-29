using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Brevis
{
    public partial class MainWindow : Window
    {
        private readonly Scene _scene;
        public MainWindow()
        {
            InitializeComponent();
            var bitmap = new WriteableBitmap(256, 256, 96, 96, PixelFormats.Bgr32, null);
            this.SceneImage.Source = bitmap;
            this._scene = new Scene(bitmap, 0);

            // TODO: remove me, this is temporary
            DrawLineOnScene();
        }

        private void DrawLineOnScene()
        {
            var projectionMatrix = this.GetProjectionMatrix();
            this._scene.StartDrawing();
            /*
             * First command line argument must be a path to the OFF file.
             */
            var off = new OFFParser(Environment.GetCommandLineArgs()[1]);
            foreach (var triangle3D in off.Triangles)
            {
                triangle3D.PerspectiveProjection(projectionMatrix).Draw(this._scene);
            }
            this._scene.EndDrawing();
        }

        private Matrix GetProjectionMatrix()
        {
            var ppm = Matrix.PerspectiveProjectionMatrix(1, 100);

            var viewMatrix = new Matrix(4, 4);
            viewMatrix.SetValue(0, 0, -0.447);
            viewMatrix.SetValue(0, 1, 0.894);
            viewMatrix.SetValue(0, 2, 0);
            viewMatrix.SetValue(0, 3, -0.447);

            viewMatrix.SetValue(1, 0, -0.458);
            viewMatrix.SetValue(1, 1, -0.229);
            viewMatrix.SetValue(1, 2, 0.859);
            viewMatrix.SetValue(1, 3, -0.315);

            viewMatrix.SetValue(2, 0, 0.768);
            viewMatrix.SetValue(2, 1, 0.384);
            viewMatrix.SetValue(2, 2, 0.512);
            viewMatrix.SetValue(2, 3, -4.353);

            viewMatrix.SetValue(3, 0, 0);
            viewMatrix.SetValue(3, 1, 0);
            viewMatrix.SetValue(3, 2, 0);
            viewMatrix.SetValue(3, 3, 1);
            return Matrix.Multiply(ppm, viewMatrix);
        }
    }
}
