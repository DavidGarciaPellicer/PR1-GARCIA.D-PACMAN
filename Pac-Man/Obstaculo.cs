using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pac_Man
{
    class Obstaculo
    {
        Rectangulo posicion;
        int posX;
        int posY;
        int ancho;
        int alto;

        Obstaculo()
        {
            Alto = ASCIIModels.Obstaculo.Length;
            Ancho = ASCIIModels.Obstaculo[0].Length;
        }

        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Ancho { get; private set; }
        public int Alto { get; private set; }


    }
}
