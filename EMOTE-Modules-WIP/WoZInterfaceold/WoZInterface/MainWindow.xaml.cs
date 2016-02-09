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
       // WOZInterfaceClient thalamusClient;

        public WOZManager mWOZManager;

        /* */
        public MainWindow()
        {
            InitializeComponent();
           // thalamusClient = new WOZInterfaceClient(this);

            mWOZManager = new WOZManager(this);
        }

        public void ChangeTitle(bool running) 
        {
            //this.Title = "Control Panel " + (running ? "(running)" : "(stopped)");
        }

        //public void SetParticipantDetails(int participantId, string participantName)
        //{
        //    this.txtParticipantName.Text = participantName;
        //    this.txtParticipantId.Text = participantId.ToString();
        //}

        //public void SetGameStateBox(bool running)
        //{
        //    if (running == true)
        //        this.txtGameState.Text = (string)"true";
        //    else
        //        this.txtGameState.Text = "false";
        //}

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            //if (!int.TryParse(txtParticipantId.Text, out i)) i = 0;

            mWOZManager.mThalamusClient.WOZPublisher.CompassShow();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mWOZManager.mThalamusClient.Dispose();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GetWindow(this).Close();
        }

        private void TaskScaffolding_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
