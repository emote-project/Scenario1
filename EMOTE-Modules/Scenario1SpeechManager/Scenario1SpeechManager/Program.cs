﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Scenario1SpeechManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string character = "";
            if (args.Length > 0)
            {
                character = args[0];
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmSpeechManager(character));
        }
    }
}
