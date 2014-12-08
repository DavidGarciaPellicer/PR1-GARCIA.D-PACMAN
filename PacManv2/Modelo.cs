using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManv2
{
    class Modelo
    {
        //constructor del Modelo que necesita de la posición en X, la posición en Y y el modelo ASCII a representar
        public Modelo(int posX,int posY,string[,] asciiModel)
        {
            //inicializamos los atributos del objeto Modelo con los argumentos del constructor
            this.posX = posX;
            this.posY = posY;
            this.asciiModel = asciiModel;

            //a partir del Modelo ASCII recuperamos el ancho y alto de la figura

            this.width = asciiModel.GetLength(0);
            this.height = asciiModel.GetLength(0);
            hasChanged = false;
        }

        //campos de respaldo con sus Propiedades. ***Falta comentarlas***

        private bool hasChanged;
        public bool HasChanged
        {
            get
            {
                return this.hasChanged;
            }
        }

        private int posX;
        public int PosX { 
            get
            {
                return this.posX;
            }
             set{ this.posX=value;}
        }

        private int posY;
        public int PosY
        {
            get
            {
                return this.posY;
            }
            set { this.posY = value; }
        }

        private int width;
        public int Width
        {
            get
            {
                return this.width;
            }
            set { this.width = value; }
        }

        private int height;
        public int Height
        {
            get
            {
                return this.height;
            }
            set { this.height = value; }
        }

        private string[,] asciiModel;
        public string[,] ASCIIModel
        {
            get
            {
                return this.asciiModel;
            }
            set { this.asciiModel = value; }
        }

        public void Mover(int deltaX, int deltaY)
        {
            int inicialPosXPacMan = this.posX;
            int inicialPosYPacMan = this.posY;

            //Rectangulo recObstaculo = new Rectangulo(modeloObstaculo.PlayerPosX, modeloObstaculo.PlayerPosY, modeloObstaculo.PlayerWidth, modeloObstaculo.PlayerHeight);
            //Rectangulo recPacMan = new Rectangulo(modeloPacMan.PlayerPosX, modeloPacMan.PlayerPosY, modeloPacMan.PlayerWidth, modeloPacMan.PlayerHeight); 

            int nuevaPosX = inicialPosXPacMan + deltaX;
            int nuevaPosY = inicialPosYPacMan + deltaY;

            this.posX = nuevaPosX;
            this.posY = nuevaPosY;
            //nuevaPosX = Math.Min(nuevaPosX, GetViewPortWidth() - 1); //Como maximo tope derecha para no salirnos
            //PlayerPosX = Math.Max(0, nuevaPosX);     //Como minimo 0 para no salirnos por la izquierda


            hasChanged = true;
        }

    }
}
