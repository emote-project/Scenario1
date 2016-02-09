using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioUtilities;

namespace Tester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AudioUtilities.Mixer.ConcatenateWave("r2d2.wav", "speech.wav", 1000, "out.wav");

            string str = Serializer.FromFile(@".\test.wav");
            Serializer.DeserializeToFile(str, @".\result.wav");
        }
    }
}
