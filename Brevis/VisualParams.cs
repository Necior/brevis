using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class VisualParams
    {
        public readonly Vertex3D camPos;
        public readonly Vertex3D lightPos;

        public readonly bool phong;
        public readonly bool fog;
        public readonly int fogColor;

        public readonly bool transparencyMode;
   
        public readonly double k_a = 0.3;
        public readonly double k_d = 0.3;
        public readonly double k_s = 0.3;
        public readonly double specularAlpha = 5;

        public readonly double i_aR = 0xff;
        public readonly double i_aG = 0xff;
        public readonly double i_aB = 0xff;

        public readonly double i_dR = 0x00;
        public readonly double i_dG = 0xff;
        public readonly double i_dB = 0x00;

        public readonly double i_sR = 0xff;
        public readonly double i_sG = 0x00;
        public readonly double i_sB = 0x00;
        public readonly PixelColor[,] pixels;
        
        public readonly String colorMode; /* One of: "COLOR", "TEXTURE", "RANDOM" */
        /* Did I hear someone saying ENUM? */
        public readonly int defaultColor;
        public VisualParams(Vertex3D camPos, Vertex3D lightPos, PixelColor[,] pixels)
        {
            this.camPos = camPos;
            this.lightPos = lightPos;
            this.pixels = pixels;
            this.colorMode = "COLOR";
            this.defaultColor = Const.Color.blue;
            this.phong = true;
            this.fog = false;
            this.fogColor = Const.Color.white;
            this.transparencyMode = false;
        }
    }
}
