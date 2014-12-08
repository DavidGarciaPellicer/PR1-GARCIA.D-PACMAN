using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pac_Man
{
  
    public class Rectangulo
    {


        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Rectangulo(int posX,int posY, int height,int width)
        {
            this.PosX = posX;
            this.PosY = posY;
            this.Height = height;
            this.Width = width;
        }

        public bool Intersecta(Rectangulo rectangulo1,Rectangulo rectangulo2){

            if (rectangulo2.PosX >= rectangulo1.PosX && rectangulo2.PosX <= rectangulo1.PosX + rectangulo1.Width
                && rectangulo2.PosY >= rectangulo1.PosY && rectangulo2.PosY <= rectangulo1.PosY + rectangulo1.Height)
                return false;
            else if (rectangulo2.PosX + rectangulo2.Width > rectangulo1.PosX && rectangulo2.PosX + rectangulo2.Width < rectangulo1.PosX + rectangulo1.Width
                 && rectangulo2.PosY >= rectangulo1.PosY && rectangulo2.PosY <= rectangulo1.PosY + rectangulo1.Height)
                return false;
            else if (rectangulo2.PosX >= rectangulo1.PosX && rectangulo2.PosX <= rectangulo1.PosX + rectangulo1.Width
                 && rectangulo2.PosY + rectangulo2.Height >= rectangulo1.PosY && rectangulo2.PosY + rectangulo2.Height <= rectangulo1.PosY + rectangulo1.Height)
                return false;
            else if (rectangulo2.PosX + rectangulo2.Width > rectangulo1.PosX && rectangulo2.PosX + rectangulo2.Width < rectangulo1.PosX + rectangulo1.Width
                 && rectangulo2.PosY+rectangulo2.Height >= rectangulo1.PosY && rectangulo2.PosY+rectangulo2.Height <= rectangulo1.PosY + rectangulo1.Height)
                return false;

            return true;
        }
    }
}
