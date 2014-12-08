using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManv2
{
   public static class ASCIIModels
    {
        private static string[,] _PacMan = new string[,] {                
                                                                            
               {" ","_","_"," "},                                                   
               {"|","O","O","|"},                                                    
               {"|"," "," ","|"},                                                 
               {"'","^","^","'"}  

    };

        public static string[,] PacMan { get { return _PacMan; } }

         private static string[] _Phantom = new string[]{

                @" _ _ ",                        
                @"| OO|",      
                @"|   |",       
                @"'^^^'"  

    };



         public static string[] Phantom { get { return _Phantom; } }

         private static string[] _Limite = new string[]{

                @"@@@@@", 
                @"@@@@@",                        
                @"@@@@@",          
                @"@@@@@"  

    };


         public static string[] Limite { get { return _Limite; } }

         private static string[] _Obstaculo = new string[]{

                @" _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ ",                        
                @"|                                                 |",          
                @"|_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _| "  

    };


         public static string[] Obstaculo { get { return _Obstaculo; } }
    

         private static string[] _ObstaculoLimites = new string[]{

               @"   _ _ _ _ ",                        
               @"  |       |",          
               @"  |       | ",
               @"  |       |",   
               @"  |       |",   
               @"  |       | ",
               @"  |       |",   
               @"  |       |",   
               @"  |       | ",
               @"  |       |",   
               @"  |       |",   
               @"  |       | ",
               @"  |       |",   
               @"  |       |",   
               @"  |       | ",
               @"  |       |",   
               @"  |       |",   
               @"  |       | ",
               @"  |       |",   
               @"  |       |",   
               @"  |       | ",
               @"  |       |",   
               @"  |       |",   
               @"  |       | ",
               @"  |       |",   
               @"  |       |",   
               @"  |       | ",
               @"  |       |",   
               @"  |       |",   
               @"  |       | ",
               @"  |       |",   
               @"  |       |",   
               @"  |_ _ _ _| "

      };
   
         public static string[] ObstaculoLimites { get { return _ObstaculoLimites; } }
 }
  
}

