using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WOZInterface.RobotSpeechStatus
{
    public class RobotSpeechStatusModel : ViewModelBase
    {
        private ObservableCollection<string> utteraences = new ObservableCollection<string>();
        public ObservableCollection<string> Utteraences
        {
            get
            {
                return utteraences;
            }
            set
            {
                utteraences = value;
                NotifyPropertyChanged("Utteraences");
            }
        }
        private bool speaking = false;
        public bool Speaking
        {
            get
            {
                return speaking;
            }
            set
            {
                speaking = value;
                NotifyPropertyChanged("Speaking");
            }
        }
        public RobotSpeechStatusModel() { }
    }
    /// <summary>
    /// Interaction logic for RobotBehaviourStatus.xaml
    /// </summary>
    public partial class RobotSpeechStatus : UserControl
    {
        RobotSpeechStatusModel data;

        public RobotSpeechStatus()
        {
            InitializeComponent();
        }



        public void SetUtterance(string utterance)
        {
            Dispatcher.Invoke((Action)delegate()
            {
                data.Utteraences.Insert(0,utterance);
            });
        }

        public void SetSpeaking(bool speaking)
        {
            data.Speaking = speaking;
        }

        private void lstLog_Loaded(object s, RoutedEventArgs ev)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                data = DataContext as RobotSpeechStatusModel;
                WoZOptimusPrimeMegaClient.GetInstance().SpeakStartedEvent += delegate { SetSpeaking(true); };
                WoZOptimusPrimeMegaClient.GetInstance().SpeakFinishedEvent += delegate { SetSpeaking(false); };
                WoZOptimusPrimeMegaClient.GetInstance().SpeakEvent += delegate(object sender, WoZOptimusPrimeMegaClient.SpeechEventArgs e) { SetUtterance(e.Text); };
                WoZOptimusPrimeMegaClient.GetInstance().SpeakBookmarksEvent += RobotSpeechStatus_SpeakBookmarksEvent;
            }
        }

        void RobotSpeechStatus_SpeakBookmarksEvent(object sender, WoZOptimusPrimeMegaClient.SpeechEventArgs e)
        {
            string text = "";
            foreach (string txt in e.Texts)
            {
                text = text + txt;
            }
            SetUtterance(text);
        }
    }
}
