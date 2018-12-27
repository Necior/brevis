using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            const int white = 16777215;
            this._scene.StartDrawing();
            for (var i = 0; i < 10; i++)
                this._scene.SetPixel(i, i, white);
            this._scene.EndDrawing();
        }
    }
}
