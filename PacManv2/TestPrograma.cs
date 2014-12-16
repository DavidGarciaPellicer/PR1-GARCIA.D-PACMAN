using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

namespace Daw1.DavidG.PacManv2
{
    class TestPrograma
    {
        static void Main()
        {
            //variable con la que indicamos el tiempo al cual se irá actualizando nuestro juego
            const int PeriodoTiempo = 40;
            bool repetirJuego = true;

            while (repetirJuego)
            {
                InicializarValoresPantalla();
                CreditosIniciales();
                Programa programa = new Programa();              
                programa.Inicializar();
                DibujarMatriz();

                //Con 2 temporizadores llamamos con uno al método que actualiza las psiciones del juego y con el otro al método que dibuja
                var actualizarJuego = new Timer(state => { programa.Actualizar(); }, null, 0, PeriodoTiempo);
                var dibujarJuego = new Timer(state => { DibujarMatriz(); }, null, 0, PeriodoTiempo * 2);

                //Mientras no se termine la partida continuamos en este bucle 
                while (!programa.Exit) { }
                //si la partida termina se muestran los créditos finales y se destruyen los temporizadores              
                actualizarJuego.Dispose();
                dibujarJuego.Dispose();
                repetirJuego = CreditosFinales(programa);
            }
         }

        static void InicializarValoresPantalla()
        {
            Console.Title = "Pac-Man ataca de nuevo";
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WindowWidth = 71;
            Console.WindowHeight = 51;
            CreditosIniciales();
        }
        public static void CreditosIniciales()
        {
            //Dibuja el rótulo centrado en la pantalla y arriba
            DibujarModeloASCII(
                (Console.WindowWidth - ASCIIModels.titulo[0].Length) / 2,
                3, ASCIIModels.titulo);
            Console.Read();
        }

        public static bool CreditosFinales(Programa programa)
        {
            Console.Clear();

            //Dibuja el rótulo centrado en la pantalla y arriba  
            //Si el número de Bolas Comidas coincide con el total de las bolas significa que el jugador ha ganado
            //SI no es así, el jugador ha perdido y mostramos el ASCII correspondiente
            if (Programa.bolasComidas == Programa.totalBolas)
            {
                DibujarModeloASCII(
                  (Console.WindowWidth - ASCIIModels.creditosGanador[0].Length) / 2,
                  3, ASCIIModels.creditosGanador);
            }
            else
            {
                DibujarModeloASCII(
                  (Console.WindowWidth - ASCIIModels.creditosPerdedor[0].Length) / 2,
                  3, ASCIIModels.creditosPerdedor);
            }

            Console.CursorLeft = 0;
            //se le pregunta al usuario si quiere volver a jugar. Cualquier opción fuera del sí lleva al cierre del juego
            //no he conseguido implementar esto con un CursorKeyInfo y un ReadKey(), que hubiera sido lo suyo

            Console.WriteLine("Quieres jugar otra partida? S/N (Haz tu elección y presiona ENTER)");
            while (true)
            {
                string entradaTeclado = Console.ReadLine();
                if (entradaTeclado == "s" || entradaTeclado == "S")
                    return true;
                else if (entradaTeclado == "n" || entradaTeclado == "N")
                    return false;
            }

        }

        static void DibujarModeloASCII(int x, int y, string[] asciiChars)
        {
            Console.Clear();
            for (int i = 0; i < asciiChars.Length; i++)
            {
                DibujarLinea(x, y + i, asciiChars[i]);
            }
        }

        //Dibujar una linea en posicion X,Y directamente en la pantalla
        static void DibujarLinea(int x, int y, string line)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(line);
        }

        //pinta el tablero
        public static void DibujarMatriz()
        {
            Console.Clear();
            Console.CursorTop = 0;
            Console.CursorLeft = 0;

            int numFilas = Programa.matrizPrincipal.GetLength(0);
            int numCols = Programa.matrizPrincipal.GetLength(1);

            for (int filas = 0; filas < numFilas; filas++)
            {
                //se almacena la fila en un vector de caracteres y se pinta la línea entera
                var fila = new char[numCols];
                for (int cols = 0; cols < numCols; cols++)
                {
                    fila[cols] = Programa.matrizPrincipal[filas, cols][0];
                }
                Console.Write(fila);

                Console.CursorLeft = 0;
                Console.CursorTop++;
            }
        }
        public static void DibujarMatriz(string[,] matriz)
        {
            Console.Clear();
            Console.CursorTop = 0;
            Console.CursorLeft = 0;

            int numFilas = matriz.GetLength(0);
            int numCols = matriz.GetLength(1);

            for (int filas = 0; filas < numFilas; filas++)
            {

                var fila = new char[numCols];
                for (int cols = 0; cols < numCols; cols++)
                {
                    fila[cols] = matriz[filas, cols][0];
                }
                Console.Write(fila);

                Console.CursorLeft = 0;
                Console.CursorTop++;
            }
        }

            
            
        }
    }

