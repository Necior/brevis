using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
        private Vertex3D camPos;
        private Vertex3D lightPos;
        private Vector3D lookAtVersor;
        private Vector3D camUpVersor;
        private readonly OFFParser offParser;
        private readonly PixelColor[,] texturePixels;
        private VisualParams visualParams;
        private bool initialized;

        private Vertex3D camTarget => camPos.Add(lookAtVersor);

        public MainWindow()
        {
            initialized = false;
            InitializeComponent();
            var bitmap = new WriteableBitmap(256, 256, 96, 96, PixelFormats.Bgr32, null);
            this.SceneImage.Source = bitmap;
            this._scene = new Scene(bitmap, Const.Color.black);
            /*
             * First command line argument must be a path to the OFF file.
             * Second command line argument must be a path to the texture.
             */
            offParser = new OFFParser(Environment.GetCommandLineArgs()[1]);
            texturePixels = Utils.GetPixels(new BitmapImage(new Uri(Environment.GetCommandLineArgs()[2])));
            visualParams = new VisualParams(camPos, lightPos, texturePixels);

            ResetScene();
            SetVP();
            initialized = true;
            Redraw(); /* Initial render sounds like a good idea, let's do it. */
        }

        private void Redraw()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            visualParams.camPos = camPos;
            visualParams.lightPos = lightPos;

            var projectionMatrix = this.GetProjectionMatrix();
            this._scene.ResetZbuffer();
            this._scene.StartDrawing();
            foreach (var triangle3D in offParser.Triangles)
            {
                if (Vector3D.DotProduct(triangle3D.a - camPos, triangle3D.normal) >= 0) /* Backface culling, yay! */
                    triangle3D.PerspectiveProjection(projectionMatrix).Draw(this._scene, visualParams);
            }
            this._scene.EndDrawing(visualParams.transparencyMode);

            watch.Stop();
            var fps = 1000 / watch.ElapsedMilliseconds;
            this.FpsLabel.Content = $"{watch.ElapsedMilliseconds} ms ({fps} FPS)";
        }

        

        private Matrix GetProjectionMatrix()
        {
            var perspectiveProjectionMatrix = Matrix.PerspectiveProjectionMatrix(0.0001, 100);
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
                    ResetScene();
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

        private void ResetScene()
        {
            /*
             * These are a good default values for a teapot model.
             */
            camPos = new Vertex3D(0, 1.5, 10);
            lightPos = new Vertex3D(5, 5, 5);
            lookAtVersor = new Vector3D(0, 0, -1).Normalize();
            camUpVersor = new Vector3D(0, -1, 0).Normalize();
        }

        private void SettingsChanged(object sender, RoutedEventArgs e)
        {
            if(initialized)
                SetVP();
        }

        private void SetVP()
        {
            visualParams.transparencyMode = uiTransparency.IsChecked.GetValueOrDefault(false);
            visualParams.wireframe = uiWFModeWireframe.IsChecked.GetValueOrDefault(false);
            if (uiColorModeColor.IsChecked.GetValueOrDefault(false))
                visualParams.colorMode = "COLOR";
            else if (uiColorModeRandom.IsChecked.GetValueOrDefault(false))
                visualParams.colorMode = "RANDOM";
            else
                visualParams.colorMode = "TEXTURE";
            uiFog.IsEnabled = !visualParams.wireframe;
            fogR.IsEnabled = !visualParams.wireframe;
            fogG.IsEnabled = !visualParams.wireframe;
            fogB.IsEnabled = !visualParams.wireframe;
            visualParams.fog = uiFog.IsChecked.GetValueOrDefault(false);
            visualParams.fogColor = Utils.RGB2Color(fogR.Value, fogG.Value, fogB.Value);
            offParser.MakeTransparent(30); /* TODO: add this parameter to the UI. */
            Redraw();
        }
    }
}
