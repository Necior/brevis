using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class VisualParams
    {
        public Vertex3D camPos;
        public Vertex3D lightPos;

        public bool phong;
        public bool fog;
        public int fogColor;

        public bool transparencyMode;
   
        public double k_a = 0.3;
        public double k_d = 0.3;
        public double k_s = 0.3;
        public double specularAlpha = 5;

        public double i_aR = 0xff;
        public double i_aG = 0xff;
        public double i_aB = 0xff;

        public double i_dR = 0x00;
        public double i_dG = 0xff;
        public double i_dB = 0x00;

        public double i_sR = 0xff;
        public double i_sG = 0x00;
        public double i_sB = 0x00;
        public PixelColor[,] pixels;
        public bool wireframe;
        
        public String colorMode; /* One of: "COLOR", "TEXTURE", "RANDOM" */
        /* Did I hear someone saying ENUM? */
        public readonly int defaultColor;
        public VisualParams(Vertex3D camPos, Vertex3D lightPos, PixelColor[,] pixels)
        {
            this.wireframe = true;
            this.camPos = camPos;
            this.lightPos = lightPos;
            this.pixels = pixels;
            this.colorMode = "COLOR";
            this.defaultColor = Const.Color.blue;
            this.phong = true;
            this.fog = false;
            this.fogColor = Const.Color.black;
            this.transparencyMode = false;
        }
    }
}
