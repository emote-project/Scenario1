using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Expression.Encoder.Devices;

using WebcamControl;
using System.Drawing.Imaging;
using System.IO;

namespace WOZInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            //mWOZManager = new WOZManager(this, "Eux");
        }

        public void ChangeTitle(bool running) 
        {
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            //Environment.Exit(0);
            Application.Current.Shutdown();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                WoZOptimusPrimeMegaClient.GetInstance().Dispose();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Client disconnection problem");
            }
            base.OnClosing(e);
            Environment.Exit(0);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            utteranceControl.keypressed(sender, e);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            WOZSettingsWindow.GetInstance().Show();
        }


        

    }
}
