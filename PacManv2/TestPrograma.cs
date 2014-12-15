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
                Programa programa = new Programa();
                programa.Inicializar();
                programa.DibujarMatriz();

                //Con 2 temporizadores llamamos con uno al método que actualiza las psiciones del juego y con el otro al método que dibuja
                var actualizarJuego = new Timer(state => { programa.Actualizar(); }, null, 0, PeriodoTiempo);
                var dibujarJuego = new Timer(state => { programa.DibujarMatriz(); }, null, 0, PeriodoTiempo * 2);

                //Mientras no se termine la partida continuamos en este bucle 
                while (!programa.Exit) { }
                //si la partida termina se muestran los créditos finales y se destruyen los temporizadores              
                actualizarJuego.Dispose();
                dibujarJuego.Dispose();
                repetirJuego = Programa.CreditosFinales(programa);
            }
         }
            
            
        }
    }

