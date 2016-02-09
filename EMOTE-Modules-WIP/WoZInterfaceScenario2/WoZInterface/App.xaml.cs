using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WOZInterface;
using WOZInterface.AutomaticBehaviours;

namespace WoZInterface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string charName = e.Args.Length > 0 ? e.Args[0].ToString() : "";
            new WoZOptimusPrimeMegaClient(charName);
            new AutomaticBehaviours();
            base.OnStartup(e);
        }
    }
}

