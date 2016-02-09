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

namespace WOZInterface.GameStatus
{
    public class GameStatusModel : ViewModelBase
    {
        private double economy = 0;
        public double Economy
        {
            set
            {
                economy = value;
                NotifyPropertyChanged("Economy");
            }
            get
            {
                return Math.Round(economy,0);
            }
        }
        private double environment = 0;
        public double Environment
        {
            set
            {
                environment = value;
                NotifyPropertyChanged("Environment");
            }
            get
            {
                return Math.Round(environment,0);
            }
        }
        private double wellbeing = 0;
        public double Wellbeing
        {
            set
            {
                wellbeing = value;
                NotifyPropertyChanged("Wellbeing");
            }
            get
            {
                return Math.Round(wellbeing,0);
            }
        }
        private double oil = 0;
        public double Oil
        {
            set
            {
                oil = value;
                NotifyPropertyChanged("Oil");
            }
            get
            {
                return Math.Round(oil,0);
            }
        }
        private double energy = 0;
        public double Energy
        {
            set
            {
                energy = value;
                NotifyPropertyChanged("Energy");
            }
            get
            {
                return Math.Round(energy,0);
            }
        }
        private double population = 0;
        public double Population
        {
            set
            {
                population = value;
                NotifyPropertyChanged("Population");
            }
            get
            {
                return Math.Round(population,0);
            }
        }
        private double money = 0;
        public double Money
        {
            set
            {
                money = value;
                NotifyPropertyChanged("Money");
            }
            get
            {
                return Math.Round(money,0);
            }
        }
        private double level = 0;
        public double Level
        {
            set
            {
                level = value;
                NotifyPropertyChanged("Level");
            }
            get
            {
                return level;
            }
        }
        private ObservableCollection<string> envHistory = new ObservableCollection<string>();
        public ObservableCollection<string> EnvHistory
        {
            set
            {
                envHistory = value;
                NotifyPropertyChanged("EnvHistory");
            }
            get
            {
                return envHistory;
            }
        }
        private ObservableCollection<string> ecoHistory = new ObservableCollection<string>();
        public ObservableCollection<string> EcoHistory
        {
            set
            {
                ecoHistory = value;
                NotifyPropertyChanged("EcoHistory");
            }
            get
            {
                return ecoHistory;
            }
        }
        private ObservableCollection<string> mayorHistory = new ObservableCollection<string>();
        public ObservableCollection<string> MayorHistory
        {
            set
            {
                mayorHistory = value;
                NotifyPropertyChanged("MayorHistory");
            }
            get
            {
                return mayorHistory;
            }
        }

        public GameStatusModel() { }
        public void Update(EmoteEvents.EnercitiesGameInfo gameInfo) 
        {
            Economy = gameInfo.EconomyScore;
            Environment = gameInfo.EnvironmentScore;
            Wellbeing = gameInfo.WellbeingScore;
            Oil = gameInfo.Oil;
            Energy = gameInfo.PowerProduction - gameInfo.PowerConsumption;
            Money = gameInfo.Money;
            Population = gameInfo.Population;
            Level = gameInfo.Level;
        }

        public void Reset()
        {
            Economy = 0;
            Environment = 0;
            Wellbeing = 0;
            Oil = 0;
            Energy = 0;
            Money = 0;
            Population = 0;
            Level = 0;
            EcoHistory = new ObservableCollection<string>();
            EnvHistory = new ObservableCollection<string>();
            MayorHistory = new ObservableCollection<string>();
        }

    }


    public partial class GameStatus : UserControl
    {
        Dictionary<string, List<string>> ActionHistory = new Dictionary<string, List<string>>();
        GameStatusClient client;
        GameStatusModel data;

        
        private string currentRole;
        public string CurrentRole
        {
            set
            {
                currentRole = value;
                Dispatcher.Invoke((Action)delegate() {
                    ShowCurrentRole(currentRole);
                });
            }
            get
            {
                return currentRole;
            }
        }
        

        public GameStatus()
        {
            InitializeComponent();
            data = DataContext as GameStatusModel;
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                client = GameStatusClient.Instance(this);
            }
        }


        public void AddAction(string actionDescription)
        {
            if (CurrentRole == null) return;
            Dispatcher.Invoke((Action)delegate() 
            {
                if (CurrentRole.Equals(EmoteEnercitiesMessages.EnercitiesRole.Environmentalist.ToString()))
                {
                    data.EnvHistory.Insert(0,actionDescription);
                }
                if (CurrentRole.Equals(EmoteEnercitiesMessages.EnercitiesRole.Economist.ToString()))
                {
                    data.EcoHistory.Insert(0, actionDescription);
                }
                if (CurrentRole.Equals(EmoteEnercitiesMessages.EnercitiesRole.Mayor.ToString()))
                {
                    data.MayorHistory.Insert(0, actionDescription);
                }
            });
        }

        

        public void StatusUpdate(EmoteEvents.EnercitiesGameInfo gameInfo)
        {
            CurrentRole = gameInfo.CurrentRole.ToString();
            data.Update(gameInfo);
        }

        private void ShowCurrentRole(string role)
        {
            if (role.Equals(EmoteEnercitiesMessages.EnercitiesRole.Economist.ToString()))
            {
                imgEconomist.Opacity = 1;
                imgEnvironmentalist.Opacity = 0.3;
                imgMayor.Opacity = 0.3;
            }
            if (role.Equals(EmoteEnercitiesMessages.EnercitiesRole.Environmentalist.ToString()))
            {
                imgEconomist.Opacity = 0.3;
                imgEnvironmentalist.Opacity = 1;
                imgMayor.Opacity = 0.3;
            }
            if (role.Equals(EmoteEnercitiesMessages.EnercitiesRole.Mayor.ToString()))
            {
                imgEconomist.Opacity = 0.3;
                imgEnvironmentalist.Opacity = 0.3;
                imgMayor.Opacity = 1;
            }
        }

        public void Reset()
        {
            data.Reset();
        }

    }
}
