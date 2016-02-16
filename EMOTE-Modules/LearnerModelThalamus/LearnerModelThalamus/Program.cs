using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnerModelThalamus
{
    class Program
    {
        static void Main(string[] args)
        {
            LearnerModelClient client = new LearnerModelClient();
            Console.WriteLine("\nPress a key to close...\n\n");
            Console.ReadLine();
            client.Dispose();
        }
    }
}
