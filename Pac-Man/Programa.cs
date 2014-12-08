using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KeyboardDemo;

namespace Pac_Man
{
    class Programa
    {
       Rectangulo obstaculo;
       Rectangulo PacMan;
       Modelo modeloObstaculo = new Modelo();
       Modelo modeloPacMan = new Modelo();
       static int dimensionMatriz = 16;
       static int[,] matrizPantalla = new int[dimensionMatriz, dimensionMatriz]; 
      // Programa programa;
       
        //Indica si debemos terminar el juego
        static bool Exit = false;

        //Intervalo en milisegundos que debe transcurrir entre actualizaciones del estado del modelo del juego
        const int PeriodTime = 8;

        //Inidicar si estamos utilizando un doble buffer para crear la escena y luego volcarla a la consola una vez generada
        static bool DoubleBufferMode { get { return ConsoleDobleBuffer != null; } }

        //Doble buffer para la consola
        static DoubleBuffer.buffer ConsoleDobleBuffer = null;

        //Como vamos a dibujar las lineas de un grafico ascii (depende de si se usa doble buffer)
        static Action<int, int, string, ConsoleColor, ConsoleColor> DrawAsciiModelLine
        {
            get
            {
                if (DoubleBufferMode) return DrawLineInBuffer;  //Si estamos en modo doble buffer la estrategia es dibujarla en el buffer
                return DrawLine;    //Si no la estrategia es dibujar directamente en la consola
            }
        }

        //Como dibujar una linea en posicion x,y -> dibujarla en el buffer
        static Action<int, int, string, ConsoleColor, ConsoleColor> DrawLineInBuffer = (i, j, linea, foregroundColor, backgroundColor) =>
        {
            ConsoleDobleBuffer.Draw(linea, i, j, GetAttributeFromConsoleColors(foregroundColor, backgroundColor));
        };

        static short GetAttributeFromConsoleColors()
        {
            return (short)(16 * (int)Console.BackgroundColor + (int)Console.ForegroundColor);
        }

        static short GetAttributeFromConsoleColors(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            return (short)(16 * (int)backgroundColor + (int)foregroundColor);
        }

        static void Main()
        {
            Programa programa = new Programa();
            programa.Initialize();
            programa.Draw();

            //Activar los temporizadores para llamar a Update y Draw cuando se cumpla el intervalo de tiempo
            var updateTimer = new Timer(state => { programa.Update(); }, null, 0, PeriodTime);
            var drawTimer = new Timer(state => { programa.Draw(); }, null, 0, PeriodTime * 2);

            //Esperar a que se pulse la tecla de salida
            while (!Exit) { }           
        }

        //Actualizar el modelo segun las entradas del usuario mediante el teclado
        void Update()
        {
            int deltaX = 0;
            int deltaY = 0;

            if (Win32API.IsKeyPressed(ConsoleKey.RightArrow)) deltaX = 1;
            if (Win32API.IsKeyPressed(ConsoleKey.LeftArrow)) deltaX = -1;
            if (Win32API.IsKeyPressed(ConsoleKey.UpArrow)) deltaY = -1;
            if (Win32API.IsKeyPressed(ConsoleKey.DownArrow)) deltaY = 1;

            if (deltaX != 0 || deltaY != 0)
            {
                    modeloPacMan.Mover(deltaX, deltaY,modeloObstaculo,modeloPacMan);
                    Console.Beep(220, 22);
                    //Console.Write("Posicion Obstaculo"+modeloObstaculo.PlayerPosX);
                    //Console.Write("Posicion Pac-Man"+modeloPacMan.PlayerPosX);
            }

              
                    

                //foreach (Obstaculo obs in Modelo.Obstaculos1)
                //{
                //if (Modelo.PlayerPosX == obs.PosX && Modelo.PlayerPosY == obs.PosY)
                //{
                //    Modelo.Mover(10, 10);
                //}
            
             

           // Modelo.MoverEnemigos();

            if (Win32API.IsKeyPressed(ConsoleKey.Escape))
                Exit = true;
        }

        //Inicializar el juego, se llama una vez para preparar los elementos del juego
        void Initialize()
        {
            Console.Title = "Pac-Man ataca de nuevo";
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;

            //Dar un tamaño a la ventana de la consola (lo mas grande posible)
            Console.WindowWidth = Console.LargestWindowWidth;
            Console.WindowHeight = Console.LargestWindowHeight;

            //Inicializar el doble buffer y el delegado para dibujar lineas segun sea el caso
            ConsoleDobleBuffer = new DoubleBuffer.buffer(Console.WindowWidth, Console.WindowHeight, Console.WindowWidth, Console.WindowHeight);
        
            //Decirle al modelo como debe obtener los valores de ancho y alto de espacio de la pantalla
            Modelo.GetViewPortWidth = () => Console.WindowWidth - 1;
            Modelo.GetViewPortHeight = () => Console.WindowHeight - 1;

            //Seleccionar la imagen ascci para representar al jugador
            modeloPacMan.Player = ASCIIModels.PacMan;
            modeloObstaculo.Player = ASCIIModels.Obstaculo;

            //Posicionar el jugador en pantalla
            modeloPacMan.Mover(
                (Modelo.GetViewPortWidth() - modeloPacMan.PlayerWidth) / 2,
                (Modelo.GetViewPortHeight() - Modelo.GetViewPortHeight() + modeloPacMan.PlayerHeight) + 10);     
        
              //Reestablecer el estado a sin cambios en el modelo, una vez se han tenido en cuenta para refrescar la vista (pantalla - Consola)

        }

        void Draw()
        {
            //Si no hay cambios en el modelo volver sin hacer nada
            if (!Modelo.HasChanged) return;

            if (DoubleBufferMode)
                ConsoleDobleBuffer.Clear(GetAttributeFromConsoleColors()); //Aplicar este metodo para limpiar el doble buffer (donde se crea la escena)
            else
                Console.Clear();  //Cuando se dibuja directamente en la consola en lugar del doble buffer

            //Dibujar el siguiente frame a mostrar en la pantalla
            this.DrawFrame();

            if (DoubleBufferMode) ConsoleDobleBuffer.Print(); //Volcar el contenido del buffer en la pantalla (Consola) si usamos doble buffer

            //Cuando se termina el resfreco de pantalla la escena esta actualizada  
            Modelo.ResetChanges();
        }

        private void DrawFrame()
        {

           
            string[] titulo = new string[] {
                    @" _____   ______   _ _ _          _    _   ______   _      _     ",
                    @"|  _  | |  __  | |  _ _|        | \  / | |  __  | | \    | |    ",
                    @"| |_| | | |__| | | |     _ _ _  |  \/  | | |__| | |  \   | |    ",
                    @"|  _ _| |  __  | | |    |_ _ _| | |\/| | |  __  | | |\ \ | |    ",
                    @"| |     | |  | | | |_ _         | |  | | | |  | | | | \ \| |    ",
                    @"|_|     |_|  |_| |_ _ _|        |_|  |_| |_|  |_| |_|  \_ _|    ",
                    @"                                                                ",
                    @"                                        by Poni Developments    "};

            pintarMatriz(rellenarMatriz());

            //Dibujar el rotulo de Demo Teclado centrado en la pantalla y arriba del todo
            DrawASCIIModel(
                (Console.WindowWidth - titulo[0].Length) / 2,
                3, titulo,
                ConsoleColor.Yellow, Console.BackgroundColor);
            
            //Dibuja a Pac-Man
            DrawASCIIModel(modeloPacMan.PlayerPosX, modeloPacMan.PlayerPosY, ASCIIModels.PacMan, ConsoleColor.Yellow, Console.BackgroundColor, ref PacMan);
            //Dibuja obstáculos y enemigos        
          //  int contador=0;
            modeloObstaculo.PlayerPosX = 20;
            modeloObstaculo.PlayerPosY = 60;
            DrawASCIIModel(modeloObstaculo.PlayerPosX, modeloObstaculo.PlayerPosY, ASCIIModels.Obstaculo, ConsoleColor.Blue, Console.BackgroundColor, ref obstaculo);
       

            //foreach (Obstaculo obs in Modelo.Obstaculos1)
            //{
            //    if (contador < 3)
            //    {
            //        DrawASCIIModel(x, y, ASCIIModels.Obstaculo, ConsoleColor.Blue, Console.BackgroundColor);
            //        x += 70;
            //    }
            //    else
            //    {
            //        if (contador == 3)
            //        {
            //            y = 56; x = 15;
            //        }

            //        DrawASCIIModel(x, y, ASCIIModels.Obstaculo, ConsoleColor.Blue, Console.BackgroundColor);
            //        x += 70;
            //    }

            //    contador++;
            //}
                //else if (contador == 3)
                //{
                //    x = 7; y = 13;
                //    DrawASCIIModel(x, y, ASCIIModels.ObstaculoLimites, ConsoleColor.Blue, Console.BackgroundColor);
                //}
                //else
                //{
                //    x = 210;
                //    DrawASCIIModel(x, y, ASCIIModels.ObstaculoLimites, ConsoleColor.Blue, Console.BackgroundColor);
                //}
             
           

            //Dibujar el jugador
            
           
        }


        //Dibuja un modelo Ascii en la posicion X,Y indicada en el doble buffer o directamente en la consola
        static void DrawASCIIModel(int x, int y, string[] asciiChars, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            for (int i = 0; i < asciiChars.Length; i++)
            {
                DrawAsciiModelLine(x, y + i, asciiChars[i], foregroundColor, backgroundColor);
            }
        }

        static void DrawASCIIModel(int x, int y, string[] asciiChars, ConsoleColor foregroundColor, ConsoleColor backgroundColor,ref Rectangulo rectangulo)
        {
            for (int i = 0; i < asciiChars.Length; i++)
            {
                DrawAsciiModelLine(x, y + i, asciiChars[i], foregroundColor, backgroundColor);
            }

            rectangulo = new Rectangulo(x,y,asciiChars.Length,asciiChars[0].Length);
        }


        //Dibujar una linea en posicion X,Y directamente en la pantalla sin usar doble buffer
        static void DrawLine(int x, int y, string line, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            ConsoleColor prevForegroundColor = Console.ForegroundColor;
            ConsoleColor prevBackgroundColor = Console.BackgroundColor;

            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;

            Console.SetCursorPosition(x, y);
            Console.Write(line);

            Console.ForegroundColor = prevForegroundColor;
            Console.BackgroundColor = prevBackgroundColor;
        }

        static int[,] rellenarMatriz()
        {
            for (int filas = 0; filas < matrizPantalla.GetLength(0); filas++)
            {
                for (int cols = 0; cols < matrizPantalla.GetLength(1); cols++)
                {
                    if (filas == 0 || filas == (matrizPantalla.GetLength(0)-1))
                        matrizPantalla[filas, cols] = 0; //0 valor de los limites
                }
            }

            return matrizPantalla;
        }

        static void pintarMatriz(int[,] matriz)
        {
            for (int filas = 0; filas < matrizPantalla.GetLength(0); filas++)
            {
                for (int cols = 0; cols < matrizPantalla.GetLength(1); cols++)
                {
                    Console.Write(matrizPantalla[filas,cols]);
                }
            }
        }
    }
}
