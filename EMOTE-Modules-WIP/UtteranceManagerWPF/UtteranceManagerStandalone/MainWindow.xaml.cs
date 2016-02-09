using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using UtteranceManager;

namespace UtteranceManagerClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        UtteranceClient utteranceClient;
        public MainWindow()
        {
            string[] args = Environment.GetCommandLineArgs();
            InitializeComponent();
            utteranceClient = new UtteranceClient("emote");
            uttManager.PerformUtteranceClicked += uttManager_PerformClicked;
            uttManager.CancelUtterance+=uttManager_CancelUtterance;
            if (Properties.Settings.Default.Autoload)
            {
                uttManager.LoadLibrary(Properties.Settings.Default.AutoloadPath);
                uttManager.AutoLoad = true;
            }
        }

        private void uttManager_CancelUtterance()
        {
            utteranceClient.UtterancePublisher.CancelUtterance("");
        }

        void uttManager_PerformClicked(object sender, UtteranceManagerControl.EMOTEUtterancesManager.PerformUtteranceEventArgs e)
        {
            Console.WriteLine(e.Utterance);
            utteranceClient.UtterancePublisher.PerformUtterance("",e.Utterance, e.Category);
        }

        protected override void OnClosed(EventArgs e)
        {
            Properties.Settings.Default.Autoload = uttManager.AutoLoad;
            Properties.Settings.Default.AutoloadPath = uttManager.LibraryPath;
            Properties.Settings.Default.Save();
            base.OnClosed(e);
        }

        private void uttManager_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
