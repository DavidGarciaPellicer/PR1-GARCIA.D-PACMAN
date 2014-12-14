using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

namespace PacManv2
{
    class Programa
    {
        /*La estrategia del juego es pintar una matriz de 50 por 70 y rellenarla con todos los
         elementos del juego */

        //variable con la que indicamos el tiempo al cual se irá actualizando nuestro juego
        const int PeriodoTiempo=30;
        //con esta variable indicamos si hay que salir del juego
        public bool Exit = false;
       
        //matriz donde vamos redibujando nuestro juego con todos sus elementos
       static string[,] matrizPrincipal;

        //variable donde se guardará el número de bolas comidas por el jugador
       int bolasComidas = 0;

        //controlamos si es un movimiento ha habido o no una bola comida
       static bool haComidoBola = false;
 
       static int totalBolas = 26;

        //vector donde se guardan las posiciones de las bolas comidas
       int[] posicionesBolasComidas = new int[totalBolas*2];

       Modelo pacMan;
       int width;
       int height;
       int posX;
       int posY;

       int inicioXASCII = 0;
       int inicioYASCII = 0;

       Modelo enemigo1;
       Modelo enemigo2;
       Modelo enemigo3;

        //para controlar la dirección hacia la que se mueven los fantasmas
       int direccionMovimientoEnemigo1=1;
       int direccionMovimientoEnemigo2=1;
       int direccionMovimientoEnemigo3=1;

       public void Inicializar()
       {
           Console.Title = "Pac-Man ataca de nuevo";
           Console.CursorVisible = false;
           Console.BackgroundColor = ConsoleColor.Black;
           Console.ForegroundColor = ConsoleColor.Yellow;
           Console.WindowWidth = 71;
           Console.WindowHeight = 51;

           CreditosIniciales();
           //rellena la matriz del juego sin PacMan ni fantasmas
           RellenarMatrizInicial();

           int posXInicioPacMan = 2;
           int posYInicioPacMan = 2;
           
           //llamamos al constructor del Modelo. Los 2 primeros argumentos para indicar la posición y el tercero para el modelo ASCII a dibujar
           pacMan = new Modelo(posXInicioPacMan, posYInicioPacMan, ASCIIModels.PacMan);

           //guardamos las Propiedades del Modelo
           width = pacMan.Width;
           height = pacMan.Height;
           posX = pacMan.PosX;
           posY = pacMan.PosY;

           //construimos los enemigos llamando al constructor de la clase Modelo igual que con el anterior pacMan
           enemigo1 = new Modelo(17,2,ASCIIModels.Phantom);
           enemigo2 = new Modelo(27, 30, ASCIIModels.Phantom);
           enemigo3 = new Modelo(40, 50, ASCIIModels.Phantom);
       }

       public void DibujarMatriz()
       {
           Console.Clear();
           Console.CursorTop = 0;
           Console.CursorLeft = 0;

           int numFilas = matrizPrincipal.GetLength(0);
           int numCols = matrizPrincipal.GetLength(1);

           for (int filas = 0; filas < numFilas; filas++)
           {
               //se almacena la fila en un vector de caracteres y se pinta la línea entera
               var fila = new char[numCols];
               for (int cols = 0; cols < numCols; cols++)
               {
                   fila[cols] = matrizPrincipal[filas, cols][0];
               }
               Console.Write(fila);

               Console.CursorLeft = 0;
               Console.CursorTop++;
           }
       }

       //Actualizar el modelo segun las entradas del usuario mediante el teclado   
       public void Actualizar()
       {
           int deltaX = 0;
           int deltaY = 0;

           if (Win32API.IsKeyPressed(ConsoleKey.RightArrow)) deltaY = 1;
           if (Win32API.IsKeyPressed(ConsoleKey.LeftArrow)) deltaY = -1;
           if (Win32API.IsKeyPressed(ConsoleKey.UpArrow)) deltaX = -1;
           if (Win32API.IsKeyPressed(ConsoleKey.DownArrow)) deltaX = 1;

           if (deltaX != 0 || deltaY != 0)
               pacMan.Mover(deltaX, deltaY);

           ActualizarMatriz();
       }

       private void ActualizarMatriz()
       {
           //si PacMan encuentra algún obstaculo lo indicaremos la variable de instancia del modelo haEncontradoObstaculo
           //si es true dibujará la matriz del movimiento anterior que es matrizPantallaAnterior

           //primero volvemos a dejar matrizPantalla como estaba originalmente(solamente el tablero sin personajes)
           RellenarMatrizInicial();

           //añadimos los enemigos
           AñadirEnemigo(enemigo1);
           AñadirEnemigo(enemigo2);
           AñadirEnemigo(enemigo3);


           int posBolaX = 0;
           int posBolaY = 0;

           posBolaX = 0;

           //si el jugador encuentra algún obstáculo vuelve a su posición anterior
           if (EncontrarObstaculo(pacMan.PosX, pacMan.PosY, 0))
           {
               pacMan.PosX = pacMan.InicialPosX;
               pacMan.PosY = pacMan.InicialPosY;
           }

           //ahora ya recorremos la matrizPantalla y dibujamos a PacMan en las posiciones actualizadas
           for (int filas = 0; filas < width; filas++)
           {
               for (int cols = 0; cols < height; cols++)
               {
                   //PacMan se come una bola cuando su posición coincide con una posición de la matriz que contiene "*"
                   //Con haComidoBola se controla también que solamente se puede comer una bola en un movimiento o actualización del juego
                   //la primera vez que se entra en la condición cambia el valor del booleano. Guarda también la posición en matriz de la bola comida y suma en el contador bolasComidas.
                   if (haComidoBola == false && (matrizPrincipal[filas + pacMan.PosX, cols + pacMan.PosY] == "*"))
                   {
                       bolasComidas++;
                       haComidoBola = true;

                       //si se ha comido todas las bolas terminamos el juego
                       if (bolasComidas == totalBolas) Exit = true;

                       //vaciamos la posición donde estaba la bola(se la ha comido)
                       posBolaX = filas + pacMan.PosX;
                       posBolaY = cols + pacMan.PosY;
                       matrizPrincipal[posBolaX, posBolaY] = " ";

                       //se recorren ahora las posiciones del vector posicionesBolasComidas y se almacena en la primera posición par que se encuentre y que además esté vacía (igual a 0)
                       //si la encuentra guarda las posiciones de la bola comida y sale del bucle
                       for (int k = 0; k < posicionesBolasComidas.Length; k++)
                       {
                           if (k % 2 == 0 && posicionesBolasComidas[k] == 0)
                           {
                               posicionesBolasComidas[k] = posBolaX;
                               posicionesBolasComidas[++k] = posBolaY;
                               break;
                           }
                       }

                   }

                   //comprobamos que las posiciones del modeloASCII no estan fuera de los rangos de los índices de su matriz 4x4
                   if (inicioXASCII < 4 && inicioYASCII < 4)
                   {
                       matrizPrincipal[filas + pacMan.PosX, cols + pacMan.PosY] = pacMan.ASCIIModel[inicioYASCII, inicioXASCII];
                       inicioXASCII++;
                   }
               }

               //Después del primer bucle for que recorre las columnas, pasamos a la siguiente fila en el modeloASCII incrementando la Y y volviendo asignar 0 a X
               inicioYASCII++;
               inicioXASCII = 0;
           }

           //reinicializa valores una vez ya hemos recorrido los 2 bucles for
           inicioYASCII = 0;
           inicioXASCII = 0;
           haComidoBola = false;

           //movemos a los fantasmas
           direccionMovimientoEnemigo1 = MoverEnemigos(enemigo1, direccionMovimientoEnemigo1);
           direccionMovimientoEnemigo2 = MoverEnemigos(enemigo2, direccionMovimientoEnemigo2);
           direccionMovimientoEnemigo3 = MoverEnemigos(enemigo3, direccionMovimientoEnemigo3);

           //si alguno de los enemigos choca con nuestro jugador la partida termina
           if (Modelo.Intersecta(pacMan, enemigo1) || Modelo.Intersecta(pacMan, enemigo2) || Modelo.Intersecta(pacMan, enemigo3)) Exit = true;

       }

       public static void CreditosIniciales()
       {
         
           string[] titulo = new string[] {
                    @" _____   ______   _ _ _          _    _   ______   _      _     ",
                    @"|  _  | |  __  | |  _ _|        | \  / | |  __  | | \    | |    ",
                    @"| |_| | | |__| | | |     _ _ _  |  \/  | | |__| | |  \   | |    ",
                    @"|  _ _| |  __  | | |    |_ _ _| | |\/| | |  __  | | |\ \ | |    ",
                    @"| |     | |  | | | |_ _         | |  | | | |  | | | | \ \| |    ",
                    @"|_|     |_|  |_| |_ _ _|        |_|  |_| |_|  |_| |_|  \_ _|    ",
                    @"                                                                ",
                    @"                                        by Poni Developments    ",
                    @"                                                                ",
                    @"                                                                ",
                    @"                                                                ",
                    @"                     Pulsa enter para comenzar                  "};

        
           //Dibuja el rótulo centrado en la pantalla y arriba
           DibujarModeloASCII(
               (Console.WindowWidth - titulo[0].Length) / 2,
               3, titulo);
              Console.Read();
       }

       public static bool CreditosFinales(Programa programa)
       {
           string[] creditosPerdedor = new string[] {

                     @"  ________    _____      _____   ___________ ",  
                     @" /  _____/   /  _  \    /     \ \_   _____/  ", 
                     @"/   \  ___  /  /_\  \  /  \ /  \ |    __)_   ", 
                     @"\    \_\  \/    |    \/    Y    \|        \  ", 
                     @" \______  /\____|__  /\____|__  /_______  /  ", 
                     @"        \/         \/         \/        \/   ", 
                     @"  ____________   _________________________   ", 
                     @"  \_____  \   \ /   /\_   _____/\______   \  ", 
                     @"   /   |   \   Y   /  |    __)_  |       _/  ", 
                     @"  /    |    \     /   |        \ |    |   \  ", 
                     @"  \_______  /\___/   /_______  / |____|_  /  ", 
                     @"          \/                 \/         \/   ",
                     @"                                             ",
                     @"                                             ",
                     @"                                             ",
                     @"                                            "};

           string[] creditosGanador = new string[] {

              @"                                       ", 
              @" ,--.   ,--..-'),-----.  ,--. ,--.     ", 
              @"  \  `.'  /( OO'  .-.  ' |  | |  |     ", 
              @".-')     / /   |  | |  | |  | | .-')   ", 
              @"OO  \   /  \_) |  |\|  | |  |_|( OO )  ", 
              @"|   /  /\_   \ |  | |  | |  | | `-' /  ", 
              @"`-./  /.__)   `'  '-'  '('  '-'(_.-'   ", 
              @"  `--'          `-----'   `-----'      ",
              @"   (`\ .-') /`            .-') _       ", 
              @"    `.( OO ),'           ( OO ) )      ", 
              @" ,--./  .--.  ,-.-') ,--./ ,--,'       ", 
              @" |      |  |  |  |OO)|   \ |  |\       ", 
              @" |  |   |  |, |  |  \|    \|  | )      ", 
              @" |  |.'.|  |_)|  |(_/|  .     |/       ", 
              @" |         | ,|  |_.'|  |\    |        ", 
              @" |   ,'.   |(_|  |   |  | \   |        ", 
              @" '--'   '--'  `--'   `--'  `--'        ",
              @"                                       ", 
              @"                                       ", 
              @"                                       "};

                                                                               
         Console.Clear();                                                      
                                                                               
         //Dibuja el rótulo centrado en la pantalla y arriba  
         //Si el número de Bolas Comidas coincide con el total de las bolas significa que el jugador ha ganado
         //SI no es así, el jugador ha perdido y mostramos el ASCII correspondiente
         if(programa.bolasComidas==totalBolas){
             DibujarModeloASCII(                                                   
               (Console.WindowWidth - creditosGanador[0].Length) / 2,                   
               3, creditosGanador);  
         }else{
             DibujarModeloASCII(                                                   
               (Console.WindowWidth - creditosPerdedor[0].Length) / 2,                   
               3, creditosPerdedor);  
         }                                      

           Console.CursorLeft = Console.WindowWidth / 3;
           //se le pregunta al usuario si quiere volver a jugar. Cualquier opción fuera del sí lleva al cierre del juego
           //no he conseguido implementar esto con un CursorKeyInfo y un ReadKey(), que hubiera sido lo suyo

               Console.WriteLine("Quieres jugar otra partida? S/N");
               while (true)
               {
                   string entradaTeclado = Console.ReadLine();
                   if (entradaTeclado == "s" || entradaTeclado == "S")
                       return true;
                   else if (entradaTeclado == "n" || entradaTeclado == "N")
                       return false;
               }

       }  

        private void AñadirEnemigo(Modelo enemigo)
        {
            for (int filas = 0; filas < enemigo.Height; filas++)
            {
                for (int cols = 0; cols < enemigo.Width; cols++)
                {
                    matrizPrincipal[filas + enemigo.PosX, cols + enemigo.PosY] = enemigo1.ASCIIModel[filas, cols];
                }
            }
        }

        //método para mover enemigos. Argumento de entrada un objeto Modelo.       
        private int MoverEnemigos(Modelo enemigo,int direccionMovimiento)
        {          
            //el movimiento es horizontal con una dirección fija hacia la derecha o a la izquierda. Si encuentra un obstáculo se mueve en la dirección horizontal contraria
            //para ello comprueba primero el valor de direccionMovimientoEnemigo; si es 1 se está moviendo hacia la derecha y si es -1 hacia la izquierda
            //comprueba en el método EncontrarObstaculo si puede seguir móviendose en esa dirección. Si no puede,cambia el valor direccionMovimientoEnemigo para que vaya en la dirección contraria

            if (direccionMovimiento == 1 && !EncontrarObstaculo(enemigo.PosX, enemigo.PosY, direccionMovimiento))
                enemigo.PosY++;
            else
            {
                direccionMovimiento = -1;
                if (!EncontrarObstaculo(enemigo.PosX, enemigo.PosY, -1))
                    enemigo.PosY--;
                else direccionMovimiento = 1;
            }

            return direccionMovimiento;
        }

        //este método tiene 3 argumentos de entrada. La posición del Modelo y el incremento de su posición.
        //En PacMan el incremento siempre es de 0, pero en los enemigos el incremento varía entre 1 y -1 dependiendo de la dirección de su movimiento
        private bool EncontrarObstaculo(int posModeloX,int posModeloY,int incremento)
        {

            for (int filas = 0; filas < width; filas++)
            {
                for (int cols = 0; cols < height; cols++)
                {
                    //si se encuentra con algún obstáculo cambiamos el valor de encontrarObstaculo
                    if (matrizPrincipal[filas+posModeloX,cols+posModeloY+incremento] == "@")
                    {
                        //si encuentra un obstaculo devolvemos la matriz con la posiciones anteriores
                        return true;
                        //matrizPantalla = guardarMatrizAnterior(matrizPantallaAnterior);
                        //return matrizPantalla;
                    }
                }
            }

            return false;

        }

        //rellena la matriz principal de 2 dimensiones  para dibujar el tablero del juego
        void RellenarMatrizInicial()
        {

            matrizPrincipal = new string[,]{
                                {"@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@"," "," "," "," "," ","*"," "," "," "," "," "," "," "," "," "," "," "," "," ","*"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","*"," "," "," "," "," "," "," ","*"," "," "," "," "," "," "," ","*"," "," "," "," "," "," "," "," "," "," "," ","*"," ","@","@"},
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@"}, //10
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@"},
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@"," ","*"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","*"," "," "," "," "," "," "," "," ","*"," "," "," "," "," "," "," "," "," ","*"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","*"," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},  //20
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@"," ","*"," "," "," "," "," "," "," "," "," "," "," ","*"," "," "," "," "," "," ","*"," "," "," "," "," "," "," "," "," "," "," "," ","*"," "," "," "," "," "," "," "," "," "," "," ","*"," "," "," "," "," "," "," ","*"," "," "," "," "," "," "," ","*"," "," "," ","@","@"}, //30
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@"," "," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},                           
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"}, //40
                                {"@","@"," ","*"," "," "," "," "," "," "," "," "," "," "," ","*"," "," "," "," "," "," ","*"," "," "," "," "," "," "," "," ","*"," "," "," "," "," "," "," ","*"," "," "," "," "," "," "," ","*"," "," "," "," "," "," "," "," ","*"," "," "," "," "," "," ","*"," "," "," ","@","@"},
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@"," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@"," "," "," "," ","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"},
                                {"@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@","@"} //50
        };


            //recorremos ahora el vector que almacena las posiciones de las bolas comidas
            //actualizamos la matriz del tablero con esos valores para redibujar el tablero sin esas bolas
            for (int k = 0; k < posicionesBolasComidas.Length; k++)
            {
                if (k % 2 == 0 && posicionesBolasComidas[k] != 0)
                {
                    matrizPrincipal[posicionesBolasComidas[k], posicionesBolasComidas[k + 1]] = " ";

                }
            }

        }

        //pinta el tablero

        public void DibujarMatriz(string[,] matriz)
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

        //string[,] guardarMatriz(string[,] matrizActual)
        //{
        //    string[,] guardaMatrizNueva = new string[matrizActual.GetLength(0), matrizActual.GetLength(1)];
        //    for (int lineas = 0; lineas < matrizActual.GetLength(0); lineas++)
        //    {
        //        for (int cols = 0; cols < matrizActual.GetLength(1); cols++)
        //        {
        //            guardaMatrizNueva[lineas, cols] = matrizActual[lineas, cols];
        //        }
        //    }

        //    return guardaMatrizNueva;

        //}

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

     
    }
}
