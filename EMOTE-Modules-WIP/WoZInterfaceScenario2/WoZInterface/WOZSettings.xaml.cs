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
using System.Windows.Shapes;

namespace WOZInterface
{
    public class WOZSettings : ViewModelBase
    {
        int envGender = 1;
        public int EnvGender
        {
            get
            {
                return envGender;
            }
            set
            {
                envGender = value;
                NotifyPropertyChanged("EnvGender");
            }
        }
        int ecoGender = 1;
        public int EcoGender
        {
            get
            {
                return ecoGender;
            }
            set
            {
                ecoGender = value;
                NotifyPropertyChanged("EcoGender");
            }
        }

        public WOZSettings() { }

        /// <summary>
        /// Class for settings window events
        /// </summary>
        /// <param name="envGender">0 if female, 1 if male</param>
        /// <param name="ecoGender">0 if female, 1 if male</param>
        public WOZSettings(int envGender, int ecoGender)
        {
            EnvGender = envGender;
            EcoGender = ecoGender;
        }
    }
    public class WOZSettingsEventArg : EventArgs {
        public WOZSettings Settings;
        public WOZSettingsEventArg(WOZSettings settings){
            this.Settings = settings;
        }
    }
    /// <summary>
    /// Interaction logic for UCSettings.xaml
    /// </summary>
    public partial class WOZSettingsWindow : Window
    {
        public static event EventHandler<WOZSettingsEventArg> SettingsChanged;
        private static WOZSettingsWindow instance;

        public static WOZSettingsWindow GetInstance()
        {
            if (instance == null)
                instance = new WOZSettingsWindow();
            return instance;
        }

        private WOZSettingsWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WOZSettingsEventArg ea = new WOZSettingsEventArg(this.DataContext as WOZSettings);
            if (SettingsChanged != null) SettingsChanged(this, ea);
            this.Hide();
        }


    }
}
