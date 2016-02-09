using System;
using System.Collections.Generic;
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

namespace WOZInterface.Perceptions
{

    public class PerceptionsViewModel : ViewModelBase
    {
        bool mutualPoint = false;
        public bool MutualPoint
        {
            get
            {
                return mutualPoint;
            }
            set
            {
                mutualPoint = value;
                NotifyPropertyChanged("MutualPoint");
            }
        }
        bool mutualGaze = false;
        public bool MutualGaze
        {
            get
            {
                return mutualGaze;
            }
            set
            {
                mutualGaze = value;
                NotifyPropertyChanged("MutualGaze");
            }
        }
        bool envTouchChin = false;
        public bool EnvTouchChin
        {
            get
            {
                return envTouchChin;
            }
            set
            {
                envTouchChin = value;
                NotifyPropertyChanged("EnvTouchChin");
            }
        }
        bool ecoTouchChin = false;
        public bool EcoTouchChin
        {
            get
            {
                return ecoTouchChin;
            }
            set
            {
                ecoTouchChin = value;
                NotifyPropertyChanged("EcoTouchChin");
            }
        }
    }
    
    
    public partial class PerceptionsControl : UserControl
    {
        PerceptionsClient client;
        PerceptionsViewModel data;

        public PerceptionsControl()
        {
            InitializeComponent();
            percHeadDirection.SetTitle("Head Direction");
            percActSpk.SetTitle("Active Speaker");
            percGaze.SetTitle("Gaze to robot");
            data = DataContext as PerceptionsViewModel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
                client = new PerceptionsClient(this);
        }


        private Base.Icons GetIcon(EmoteCommonMessages.GazeEnum headDirection)
        {
            switch (headDirection)
            {
                case EmoteCommonMessages.GazeEnum.Other:
                    return Base.Icons.Elsewhere;
                case EmoteCommonMessages.GazeEnum.Robot:
                    return Base.Icons.NaoEyesOff;
                case EmoteCommonMessages.GazeEnum.ScreenLeft:
                    return Base.Icons.ScreenLeft;
                case EmoteCommonMessages.GazeEnum.ScreenRight:
                    return Base.Icons.ScreenRight;
                    
            }
            return Base.Icons.Elsewhere;
        }

        public void SetEnvHeadDirection(EmoteCommonMessages.GazeEnum headDirection)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                percHeadDirection.SetRightIcon(GetIcon(headDirection));
            }));
        }
        public void SetEcoHeadDirection(EmoteCommonMessages.GazeEnum headDirection)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                percHeadDirection.SetLeftIcon(GetIcon(headDirection));
            }));
        }


        public void SetEnvGazeToRobot(bool value)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                percGaze.SetLeftIcon(value ? Base.Icons.NaoEyesOn : Base.Icons.NaoEyesOff);
            }));
        }
        public void SetEcoGazeToRobot(bool value)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                percGaze.SetRightIcon(value ? Base.Icons.NaoEyesOn : Base.Icons.NaoEyesOff);
            }));
        }


        public void SetEnvActiveSpk(bool value)
        {
            
            this.Dispatcher.Invoke((Action)(() =>
                {
                    percActSpk.SetRightIcon(value ? Base.Icons.ActiveSpeakerON : Base.Icons.ActiveSpeakerOFF);
                }));
        }
        public void SetEcoActiveSpk(bool value)
        {
            this.Dispatcher.Invoke((Action)(() =>
                {
                    percActSpk.SetLeftIcon(value ? Base.Icons.ActiveSpeakerON : Base.Icons.ActiveSpeakerOFF);
                }));
        }


        public void SetEcoActiveSpkVolume(double value)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                percActSpk.SetRightDescription(value+" db");
                percActSpk.SetRightProgressBar(value + 50);
            }));
        }
        public void SetEnvActiveSpkVolume(double value)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                percActSpk.SetLeftDescription(value + " db");
                percActSpk.SetLeftProgressBar(value + 50);
            }));
        }


        public void SetEcoEyebrowsAUs(double au4Left, double au4Right, double au2Left, double au2Right)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                percEyebrows.economist.BrowsDefinition.OuterBrowL = au4Left;
                percEyebrows.economist.BrowsDefinition.OuterBrowR = au4Right;
                percEyebrows.economist.BrowsDefinition.InnerBrowL = au2Left;
                percEyebrows.economist.BrowsDefinition.InnerBrowR = au2Right;
                //percEyebrows.economist.BrowsDefinition.InnerBrow = au4;
            }));
        }
        public void SetEnvEyebrowsAUs(double au4Left, double au4Right, double au2Left, double au2Right)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                percEyebrows.environmentalist.BrowsDefinition.OuterBrowL = au4Left;
                percEyebrows.environmentalist.BrowsDefinition.OuterBrowR = au4Right;
                percEyebrows.environmentalist.BrowsDefinition.InnerBrowL = au2Left;
                percEyebrows.environmentalist.BrowsDefinition.InnerBrowR = au2Right;
            }));
        }

        public void SetMutualGaze(bool value)
        {
            data.MutualGaze = value;
        }

        public void SetMutualPoint(bool value)
        {
            data.MutualPoint = value;
        }

        public void SetEcoTouchChin(bool value)
        {
            data.EcoTouchChin = value;
        }

        public void SetEnvTouchChin(bool value)
        {
            data.EnvTouchChin = value;
        }


    }
}
