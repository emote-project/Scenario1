using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            MapInterfaceClient client = new MapInterfaceClient();
            Console.WriteLine("\nPress a key to close...\n\n");
            Console.ReadLine();
            client.Dispose();
        }
    }
}
