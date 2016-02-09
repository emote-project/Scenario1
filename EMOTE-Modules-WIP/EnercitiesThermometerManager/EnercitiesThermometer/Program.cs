using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnercitiesThermometer
{
    class Program
    {
        static void Main(string[] args)
        {
            string character = "";
            if (args.Length > 0)
            {
                if (args[0] == "help")
                {
                    Console.WriteLine("Usege: " + Environment.GetCommandLineArgs()[0] + " <CharacterName>");
                    return;
                }
                character = args[0];
            }
            EnercitiesThermometerClient thermometerClient = new EnercitiesThermometerClient(character);
            Console.ReadLine();
            thermometerClient.Dispose();
        }
    }
}
