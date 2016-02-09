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
using EmoteCommonMessages;
using Thalamus;
using System.ComponentModel;

namespace WOZInterface.NVB
{
    /// <summary>
    /// Interaction logic for NonVerbalBehavior.xaml
    /// </summary>
    public partial class NonVerbalBehavior : UserControl
    {
        private List<string> Targets = new List<string>(){ 
            "Click","ThroughMap","ThroughRoom"
        };
        private NVBClient client;

        public NonVerbalBehavior()
        {
            InitializeComponent();
            Targets.Sort();
            cmbTargets.ItemsSource = Targets;
            cmbTargets.SelectedIndex = 0;
            cmbTargets1.ItemsSource = cmbTargets.ItemsSource;
            cmbTargets1.SelectedIndex = 1;
            cmbTargets2.ItemsSource = cmbTargets.ItemsSource;
            cmbTargets2.SelectedIndex = 2;
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                client = new NVBClient(this);
            }

        }


        

        public void AddTarget(string targetName){
            
            this.Dispatcher.Invoke((Action)(() =>
            {
                Targets.Add(targetName);
                Targets = new List<string>(Targets.Distinct<string>());
                Targets.Sort();
                cmbTargets.Items.Refresh();
                cmbTargets1.Items.Refresh();
                cmbTargets2.Items.Refresh();
            }));
        }

        private void btmGaze_Click(object sender, RoutedEventArgs e)
        {

            WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.GazeAtTarget(cmbTargets.SelectedItem.ToString());
        }

        private void btmGlance_Click(object sender, RoutedEventArgs e)
        {
            WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.GlanceAtTarget(cmbTargets.SelectedItem.ToString());
        }

        private void btmGaze1_Click(object sender, RoutedEventArgs e)
        {
            WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.GazeAtTarget(cmbTargets1.SelectedItem.ToString());
        }

        private void btmGlance1_Click(object sender, RoutedEventArgs e)
        {
            WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.GlanceAtTarget(cmbTargets1.SelectedItem.ToString());
        }

        private void btmGaze2_Click(object sender, RoutedEventArgs e)
        {
            WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.GazeAtTarget(cmbTargets2.SelectedItem.ToString());
        }

        private void btmGlance2_Click(object sender, RoutedEventArgs e)
        {
            WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.GlanceAtTarget(cmbTargets2.SelectedItem.ToString());
        }

        private void btnNod_Click(object sender, RoutedEventArgs e)
        {
            string lexeme = (radPositive.IsChecked == true ? "NOD" : "SHAKE");
            int repetitions = 3;
            try
            {
                repetitions = int.Parse(txtRepetitions.Text);
            }
            catch (Exception) { }
            WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.Head("", lexeme, repetitions);
        }

    }
}
