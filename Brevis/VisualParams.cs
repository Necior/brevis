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
        public VisualParams(Vertex3D camPos, Vertex3D lightPos)
        {
            this.camPos = camPos;
            this.lightPos = lightPos;
        }
    }
}
