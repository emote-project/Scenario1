using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GameplayManager
{
   
    class Program
    {
        static void Main(string[] args)
        {

            string character = "";
            string pyAddress = "localhost";
            string state = "enabled";
            bool playAllRoles = false;
            if (args.Length > 0)
            {
                if (args[0] == "help")
                {
                    Console.WriteLine("Usage: " + Environment.GetCommandLineArgs()[0] + " <CharacterName> <state> <playAllRoles>"+
                       "\n  <state> may be 'enabled' or 'disabled'. When 'disabled' this module doesn't sends game actions to the game"+
                       "\n  <playAllRoles> may be 'true' or 'false'. When 'true' this module will do game moves for all players");
                    return;
                }
                character = args[0];
                if (args.Length > 1) state = args[1];
                if (args.Length > 2) playAllRoles = args[2].Equals("true") ? true : false;
                if (args.Length > 3) pyAddress = args[3];
                
            }
            GamePlayManager gamePlayManager = new GamePlayManager(character,state,playAllRoles);
            Console.ReadLine();
            gamePlayManager.Dispose();
        }
    }
}
