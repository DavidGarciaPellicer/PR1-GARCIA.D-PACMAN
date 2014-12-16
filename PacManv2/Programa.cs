using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

namespace Daw1.DavidG.PacManv2
{
    class Programa
    {
        /*La estrategia del juego es pintar una matriz de 50 por 70 y rellenarla con todos los 
         elementos del juego 
         Hay algunos bugs en el juego, sobretodo el referente al repintado del menú cuando hay que repetir la partida que a veces falla*/

        //con esta variable indicamos si hay que salir del juego
        public bool Exit = false;

        //matriz donde vamos redibujando nuestro juego con todos sus elementos
        public static string[,] matrizPrincipal;

        //variable donde se guardará el número de bolas comidas por el jugador
        public static int bolasComidas = 0;

        //controlamos si en un movimiento ha habido o no una bola comida
        static bool haComidoBola = false;
        //número de bolas en el juego
        public const int totalBolas = 26;

        //vector donde se guardan las posiciones de las bolas comidas
        int[] posicionesBolasComidas = new int[totalBolas * 2];
        
        //se declaran las variables del jugador y algunos campos de instancia para manejar sus propiedades
        Modelo pacMan;
        int width;
        int height;
        int posX;
        int posY;
        
        //para las posiciones de los modelosASCII
        int inicioXASCII = 0;
        int inicioYASCII = 0;
        
        //variables para el modelo de los fantasmas
        Modelo enemigo1;
        Modelo enemigo2;
        Modelo enemigo3;

        //para controlar la dirección hacia la que se mueven los fantasmas
        int direccionMovimientoEnemigo1 = 1;
        int direccionMovimientoEnemigo2 = 1;
        int direccionMovimientoEnemigo3 = 1;

        public void Inicializar()
        {            
            
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
            enemigo1 = new Modelo(17, 2, ASCIIModels.Phantom);
            enemigo2 = new Modelo(27, 30, ASCIIModels.Phantom);
            enemigo3 = new Modelo(40, 50, ASCIIModels.Phantom);
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

            VolverPosicionAnterior();

            ActualizarPosiciones(ref posBolaX, ref posBolaY);

            //movemos a los fantasmas
            direccionMovimientoEnemigo1 = MoverEnemigos(enemigo1, direccionMovimientoEnemigo1);
            direccionMovimientoEnemigo2 = MoverEnemigos(enemigo2, direccionMovimientoEnemigo2);
            direccionMovimientoEnemigo3 = MoverEnemigos(enemigo3, direccionMovimientoEnemigo3);

            //si alguno de los enemigos choca con nuestro jugador la partida termina
            if (Modelo.Intersecta(pacMan, enemigo1) || Modelo.Intersecta(pacMan, enemigo2) || Modelo.Intersecta(pacMan, enemigo3)) Exit = true;

        }

        private void ActualizarPosiciones(ref int posBolaX, ref int posBolaY)
        {
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
        }

        private void VolverPosicionAnterior()
        {
            //si el jugador encuentra algún obstáculo vuelve a su posición anterior
            if (EncontrarObstaculo(pacMan.PosX, pacMan.PosY, 0))
            {
                pacMan.PosX = pacMan.InicialPosX;
                pacMan.PosY = pacMan.InicialPosY;
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
        private int MoverEnemigos(Modelo enemigo, int direccionMovimiento)
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
        private bool EncontrarObstaculo(int posModeloX, int posModeloY, int incremento)
        {

            for (int filas = 0; filas < width; filas++)
            {
                for (int cols = 0; cols < height; cols++)
                {
                    //si se encuentra con algún obstáculo cambiamos el valor de encontrarObstaculo
                    if (matrizPrincipal[filas + posModeloX, cols + posModeloY + incremento] == "@")
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
        
    }
}
