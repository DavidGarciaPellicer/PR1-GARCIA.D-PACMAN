using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pac_Man
{

    class Modelo
    {
        //Posicion X e Y del jugador en la pantalla
        public int PlayerPosX = 0;
        public int PlayerPosY = 0;

        public static Obstaculo[] Obstaculos1 = new Obstaculo[6];

        //Funciones para que el programa controlador le diga al modelo como debe recuperar las dimensiones de la pantalla
        public static Func<int> GetViewPortWidth = null;
        public static Func<int> GetViewPortHeight = null;

        //El jugador
        private string[] _Player;
        public string[] Player
        {
            get { return _Player; }
            set
            {
                _Player = value;
                HasChanged = true;
            }
        }

        public int PlayerWidth { get { return this.Player[0].Length; } }
        public int PlayerHeight { get { return this.Player.Length; } }

        public void Mover(int deltaX, int deltaY)
        {
            //Si no hay movimiento volver del metodo sin hacer nada;
            if (deltaX == 0 && deltaY == 0) return;

            //int nuevaPosX = PlayerPosX + deltaX;
            //nuevaPosX = Math.Min(nuevaPosX, GetViewPortWidth() - 1); //Como maximo tope derecha para no salirnos
            //PlayerPosX = Math.Max(0, nuevaPosX);     //Como minimo 0 para no salirnos por la izquierda

            this.PlayerPosX = Math.Max(0,
                Math.Min(GetViewPortWidth() - PlayerWidth, PlayerPosX + deltaX));

            int nuevaPosY = this.PlayerPosY + deltaY;
            nuevaPosY = Math.Min(nuevaPosY, GetViewPortHeight() - (PlayerHeight - 1)); //Para no salirnos por abajo
            this.PlayerPosY = Math.Max(0, nuevaPosY);  //Para no salirnos por arriba



            HasChanged = true;
        }
        //Mover un jugador por el mundo 
        public void Mover(int deltaX, int deltaY,Modelo modeloObstaculo,Modelo modeloPacMan)
        {
            int inicialPosXPacMan = modeloPacMan.PlayerPosX;
            int inicialPosYPacMan = modeloPacMan.PlayerPosY;

            Rectangulo recObstaculo = new Rectangulo(modeloObstaculo.PlayerPosX, modeloObstaculo.PlayerPosY, modeloObstaculo.PlayerWidth, modeloObstaculo.PlayerHeight);
            Rectangulo recPacMan = new Rectangulo(modeloPacMan.PlayerPosX, modeloPacMan.PlayerPosY, modeloPacMan.PlayerWidth, modeloPacMan.PlayerHeight);

            //Si no hay movimiento volver del metodo sin hacer nada;
            if (deltaX == 0 && deltaY == 0) return;

            //int nuevaPosX = PlayerPosX + deltaX;
            //nuevaPosX = Math.Min(nuevaPosX, GetViewPortWidth() - 1); //Como maximo tope derecha para no salirnos
            //PlayerPosX = Math.Max(0, nuevaPosX);     //Como minimo 0 para no salirnos por la izquierda


            modeloPacMan.PlayerPosX = Math.Max(0,
                Math.Min(GetViewPortWidth() - PlayerWidth, PlayerPosX + deltaX));

            int nuevaPosY = modeloPacMan.PlayerPosY + deltaY;
            nuevaPosY = Math.Min(nuevaPosY, GetViewPortHeight() - (PlayerHeight - 1)); //Para no salirnos por abajo
            modeloPacMan.PlayerPosY = Math.Max(0, nuevaPosY);  //Para no salirnos por arriba

            if (recPacMan.Intersecta(recObstaculo, recPacMan))
            {
                modeloPacMan.PlayerPosX = 60;
                modeloPacMan.PlayerPosY = 60;
          
            }

            HasChanged = true;
        }
        //método para posicionar principalmente los obstáculos
        public static void Posicionar(Obstaculo Obstaculo)
        {
           
        }

        //public  static string[] Rotate(string[] matriz)
        //{
        //    string[] matrizRotada =  new string[matriz.Length]; 
        //    for(int i=0;i<matriz.Length;i++){
        //        for (int j = 0; j < matriz[1].Length;j++ )
        //        {
        //            matrizRotada[j][i]=matriz[i][j];
        //        }
        //    }
            
        //}
        //Indica si se ha producido algun cambio en el estado del modelo
        private static bool _hasChanged = true;

        public static bool HasChanged
        {
            get { return _hasChanged; }
            private set { _hasChanged = value; }
        }

        //Reestablecer el estado a sin cambios en el modelo, una vez se han tenido en cuenta para refrescar la vista (pantalla - Consola)
        public static void ResetChanges()
        {
            HasChanged = false;
        }


    }
}
