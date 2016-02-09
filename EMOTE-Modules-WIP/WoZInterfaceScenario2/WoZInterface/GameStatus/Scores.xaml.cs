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

namespace WOZInterface.GameStatus
{
    /// <summary>
    /// Interaction logic for Scores.xaml
    /// </summary>
    public partial class Scores : UserControl
    {
        GameStatusClient client;
        public double Economy
        {
            set
            {
                Dispatcher.Invoke((Action)delegate()
                {
                    txtEconomy.Text = value + "";
                });
            }
            get
            {
                return double.Parse(txtEconomy.Text);
            }
        }
        public double Environment
        {
            set
            {
                Dispatcher.Invoke((Action)delegate()
                {
                    txtEnvironment.Text = value + "";
                });
            }
            get
            {
                return double.Parse(txtEnvironment.Text);
            }
        }
        public double Wellbeing
        {
            set
            {
                Dispatcher.Invoke((Action)delegate()
                {
                    txtWellbeing.Text = value + "";
                });
            }
            get
            {
                return double.Parse(txtWellbeing.Text);
            }
        }
        public double Oil
        {
            set
            {
                Dispatcher.Invoke((Action)delegate()
                {
                    txtOil.Text = value + "";
                });
            }
            get
            {
                return double.Parse(txtOil.Text);
            }
        }
        public double Energy
        {
            set
            {
                Dispatcher.Invoke((Action)delegate()
                {
                    txtEnergy.Text = value + "";
                });
            }
            get
            {
                return double.Parse(txtEnergy.Text);
            }
        }
        public double Population
        {
            set
            {
                Dispatcher.Invoke((Action)delegate()
                {
                    txtPopulation.Text = value + "";
                });
            }
            get
            {
                return double.Parse(txtPopulation.Text);
            }
        }
        public double Level
        {
            set
            {
                Dispatcher.Invoke((Action)delegate()
                {
                    txtLevel.Text = value + "";
                });
            }
            get
            {
                return double.Parse(txtLevel.Text);
            }
        }
        public string CurrentRole
        {
            set
            {
                Dispatcher.Invoke((Action)delegate()
                {
                    txtCurrentRole.Text = value;
                });
            }
            get
            {
                return txtCurrentRole.Text;
            }
        }


        public Scores()
        {
            InitializeComponent();
        }
        public void Initialize(string thalamusCharacterName)
        {
            client = GameStatusClient.Instance(this, thalamusCharacterName);
        }
        public void StatusUpdate(EmoteEvents.EnercitiesGameInfo gameInfo)
        {
            Environment = gameInfo.EnvironmentScore;
            Economy = gameInfo.EconomyScore;
            Wellbeing = gameInfo.WellbeingScore;
            Oil = gameInfo.Oil;
            Energy = gameInfo.PowerProduction;
            Population = gameInfo.Population;
            Level = gameInfo.Level;
            CurrentRole = gameInfo.CurrentRole.ToString();
        }


    }
}
