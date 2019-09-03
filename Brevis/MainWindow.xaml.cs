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
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Brevis
{
    public partial class MainWindow : Window
    {
        private readonly Scene _scene;
        private Vector3D camPos;
        private Vector3D lookAtVersor;
        private Vector3D camUpVersor;
        private readonly OFFParser offParser;

        private Vector3D camTarget => camPos.Add(lookAtVersor);

        public MainWindow()
        {
            InitializeComponent();
            var bitmap = new WriteableBitmap(256, 256, 96, 96, PixelFormats.Bgr32, null);
            this.SceneImage.Source = bitmap;
            this._scene = new Scene(bitmap, Const.Color.black);
            /*
             * First command line argument must be a path to the OFF file.
             */
            offParser = new OFFParser(Environment.GetCommandLineArgs()[1]);

            ResetCameraPosition();
            Redraw(); /* Initial render sound like a good idea, let's do it. */
        }

        private void Redraw()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var projectionMatrix = this.GetProjectionMatrix();
            this._scene.StartDrawing();
            foreach (var triangle3D in offParser.Triangles)
            {
                triangle3D.PerspectiveProjection(projectionMatrix).Draw(this._scene, Const.Color.white);
            }
            this._scene.EndDrawing();

            watch.Stop();
            var fps = 1000 / watch.ElapsedMilliseconds;
            this.FpsLabel.Content = $"It took {watch.ElapsedMilliseconds} ms to generate a frame. FPS at this speed: {fps}";
        }

        private Matrix GetProjectionMatrix()
        {
            var perspectiveProjectionMatrix = Matrix.PerspectiveProjectionMatrix(1, 100);
            var viewMatrix = Matrix.ViewMatrix(
                camPos.x, camPos.y, camPos.z,
                camTarget.x, camTarget.y, camTarget.z,
                camUpVersor.x, camUpVersor.y, camUpVersor.z
                );
            return Matrix.Multiply(perspectiveProjectionMatrix, viewMatrix);
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            /*
             * TODO: allow for continuous update when keeping keys pressed.
             * `Keyboard.IsKeyDown` might be useful for this purpose ;-)
             */
            double stepSize = 0.1;
            double rotationAngle = Math.PI / 32;
            switch (e.Key)
            {
                case Key.Back:
                    ResetCameraPosition();
                    break;
                case Key.W:
                    MoveForward(stepSize);
                    break;
                case Key.S:
                    MoveForward(-stepSize);
                    break;
                case Key.A:
                    MoveLeft(stepSize);
                    break;
                case Key.D:
                    MoveLeft(-stepSize);
                    break;
                case Key.Left:
                    RotateLeft(rotationAngle);
                    break;
                case Key.Right:
                    RotateLeft(-rotationAngle);
                    break;
                case Key.Down:
                    RotateDown(rotationAngle);
                    break;
                case Key.Up:
                    RotateDown(-rotationAngle);
                    break;
            }
            Redraw();
        }

        private void MoveLeft(double stepSize)
        {
            /*
             * What is "left"?
             * ---------------
             *
             * We have two vectors: camUpVersor and lookAtVersor.
             * We want 3rd vector, namely left.
             * Vector has a direction (direction as a line and its sense) and a length.
             *
             * We will calculate its length by normalizing the vector and multiplying by stepSize.
             *
             * `left` is just a perpendicular vector :-)
             */
            var left = Vector3D.PerpendicularVector(camUpVersor, lookAtVersor).Normalize();
            camPos = camPos.Add(left.Mul(stepSize));
        }

        private void MoveForward(double stepSize)
        {
            /*
             * What is "forward"?
             * ------------------
             *
             * Move stepSize into lookAt direction.
             */
            camPos = camPos.Add(new Vector3D(lookAtVersor.x * stepSize, lookAtVersor.y * stepSize, lookAtVersor.z * stepSize));
        }

        private void RotateLeft(double angle)
        {
            RotateCamera(camUpVersor, angle);
        }

        private void RotateDown(double angle)
        {
            var perpendicular = Vector3D.PerpendicularVector(lookAtVersor, camUpVersor).Normalize();
            RotateCamera(perpendicular, angle);
        }

        private void RotateCamera(Vector3D rotationVector, double angle)
        {
            var lookAtMatrix = lookAtVersor.ToMatrix();
            var camUpMatrix = camUpVersor.ToMatrix();
            lookAtMatrix = Matrix.Multiply(
                Matrix.RotationMatrix(rotationVector, angle),
                lookAtMatrix
            );
            camUpMatrix = Matrix.Multiply(
                Matrix.RotationMatrix(rotationVector, angle),
                camUpMatrix
            );
            lookAtVersor = Vector3D.FromMatrix(lookAtMatrix).Normalize();
            camUpVersor = Vector3D.FromMatrix(camUpMatrix).Normalize();
        }

        private void ResetCameraPosition()
        {
            /*
             * These are a good default values for a teapot model.
             */
            camPos = new Vector3D(0, 0, 10);
            lookAtVersor = new Vector3D(0, 0, -1).Normalize();
            camUpVersor = new Vector3D(0, -1, 0).Normalize();
        }
    }
}
