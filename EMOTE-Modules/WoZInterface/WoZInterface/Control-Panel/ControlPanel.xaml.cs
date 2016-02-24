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

namespace WOZInterface
{
    /// <summary>
    /// Interaction logic for ControlPanel.xaml
    /// </summary>
    public partial class ControlPanel : UserControl
    {
        public WOZManager mWOZManager;
        
        bool mIsRunning = false;        
        public ControlPanel(WOZManager wozManager)
        {
            mWOZManager = wozManager;
            InitializeComponent();
        }

        /*
         * This returns the status of the system i.e. running or not... 
         */
        bool SetRunningStatus(bool running)
        {        
            if (running)
            {
                if(mIsRunning == false)
                {
                    var uri = new Uri("pack://application:,,,/Images/Generic/RunningOn.png");
                    var bitmap = new BitmapImage(uri);
                    StatusIndicator.Source = bitmap;
                    statusLabel.Content = "Started!";

                    mIsRunning = true;
                    return true;
                }
                else
                {
                    MessageBox.Show("The task is already running, try stopping it first!");
                    return false;
                }   
            }
            else
            {
                var uri = new Uri("pack://application:,,,/Images/Generic/RunningOff.png");
                var bitmap = new BitmapImage(uri);
                StatusIndicator.Source = bitmap;
                statusLabel.Content = "Stopped!";

                mIsRunning = false;
                return true;

            }
        }

        /*
         * Event Handlers
         */
        
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (txtPId.Text.Length == 0)
            {
                MessageBox.Show("ERROR! : Please Check the Participant Name or ID");
                return;
            }
            else if(SetRunningStatus(true))
            {
                int i;
                if (!int.TryParse(txtPId.Text, out i)) i = 0;

                mWOZManager.WOZStart(txtPName.Text, txtPId.Text);
            }
            
            
            
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            mWOZManager.WOZStop();
            SetRunningStatus(false);
        }
    }
}
