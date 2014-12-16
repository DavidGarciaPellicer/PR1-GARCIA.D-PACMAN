using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daw1.DavidG.PacManv2
{
   public static class ASCIIModels
    {
       private static string[,] _PacMan = new string[,] {                
                                                                            
              
                {" ","_","_"," "},                        
                {"|","O","_","|"},      
                {"|","|","_"," "},       
                {"|","_","_","|"}  

    };

        public static string[,] PacMan { get { return _PacMan; } }

        private static string[,] _Phantom = new string[,]{
          
               {" ","_","_"," "},                                                   
               {"|","O","O","|"},                                                    
               {"|"," "," ","|"},                                                 
               {"'","^","^","'"}  


    };

        public static string[,] Phantom { get { return _Phantom; } }

        public static string[] titulo = new string[] {
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

        public static string[] creditosPerdedor = new string[] {

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

        public static string[] creditosGanador = new string[] {

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


    //     private static string[] _Obstaculo = new string[]{

    //            @" _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ ",                        
    //            @"|                                                 |",          
    //            @"|_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _| "  

    //};


    //     public static string[] Obstaculo { get { return _Obstaculo; } }
    

    //     private static string[] _ObstaculoLimites = new string[]{

    //           @"   _ _ _ _ ",                        
    //           @"  |       |",          
    //           @"  |       | ",
    //           @"  |       |",   
    //           @"  |       |",   
    //           @"  |       | ",
    //           @"  |       |",   
    //           @"  |       |",   
    //           @"  |       | ",
    //           @"  |       |",   
    //           @"  |       |",   
    //           @"  |       | ",
    //           @"  |       |",   
    //           @"  |       |",   
    //           @"  |       | ",
    //           @"  |       |",   
    //           @"  |       |",   
    //           @"  |       | ",
    //           @"  |       |",   
    //           @"  |       |",   
    //           @"  |       | ",
    //           @"  |       |",   
    //           @"  |       |",   
    //           @"  |       | ",
    //           @"  |       |",   
    //           @"  |       |",   
    //           @"  |       | ",
    //           @"  |       |",   
    //           @"  |       |",   
    //           @"  |       | ",
    //           @"  |       |",   
    //           @"  |       |",   
    //           @"  |_ _ _ _| "

    //  };
   
    //     public static string[] ObstaculoLimites { get { return _ObstaculoLimites; } }

    //     private static string[] _copia = new string[]{

    //            @" _ _ ",                        
    //            @"| OO|",      
    //            @"|   |",       
    //            @"'^^^'"  

    //};

 }
  
}

