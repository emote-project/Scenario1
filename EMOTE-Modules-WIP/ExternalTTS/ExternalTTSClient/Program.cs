using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThalamusSpeechClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ExternalTTS.ExternalTTS client = new ExternalTTS.ExternalTTS();
            Console.WriteLine("\nPress a key to close...\n\n");
            Console.ReadLine();
            client.Dispose();
        }

    }
}
