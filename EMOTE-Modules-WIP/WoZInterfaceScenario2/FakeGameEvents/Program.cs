using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeGameEvents
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new FakeGameEventsClient();
            Console.ReadLine();
            client.Dispose();
        }
    }
}
