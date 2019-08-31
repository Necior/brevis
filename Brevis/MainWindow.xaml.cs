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
            this._scene = new Scene(bitmap, Const.Color.black);

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
            var perspectiveProjectionMatrix = Matrix.PerspectiveProjectionMatrix(1, 100);
            var viewMatrix = Matrix.ViewMatrix(3, 2, 2.5, 0, 0.5, 0.5, 0, 0, 1);
            return Matrix.Multiply(perspectiveProjectionMatrix, viewMatrix);
        }
    }
}
