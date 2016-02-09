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
using EmoteEvents;

namespace WOZInterface.GameStrategy.Common
{
    /// <summary>
    /// Interaction logic for GameStrategyManager.xaml
    /// </summary>
    public partial class AIStrategy : UserControl
    {
        public event EventHandler<StrategyUpdatedEventArgs> Updated;
        public class StrategyUpdatedEventArgs : EventArgs
        {
            public double[] Weights;
            public StrategyUpdatedEventArgs(double[] weights)
            {
                this.Weights = weights;
            }
        }

        private const int SCALE = 1000;

        private Strategy strategy = new Strategy();
        public Strategy Strategy
        {
            get
            {
                return strategy;
            }
            set
            {
                strategy = value;
                UpdateSliders();
            }
        }

        private bool shouldCallUpdate = false; // set to trye when the slider values are changed. The cycle checks this variable to call the update event.
        private bool autoUpdatingSliders = false;

        public AIStrategy()
        {
            InitializeComponent();
            this.DataContext = this;
            UpdateCycle();
        }

        public void SetEditable(bool editable)
        {
            sldEconomy.IsEnabled = editable;
            sldEnvironment.IsEnabled = editable;
            sldWellbeing.IsEnabled = editable;
            sldMoney.IsEnabled = editable;
            sldPower.IsEnabled = editable;
            sldOil.IsEnabled = editable;
            sldHouses.IsEnabled = editable;
        }

        private void UpdateSliders()
        {
            Dispatcher.BeginInvoke((Action)delegate() 
            {
                autoUpdatingSliders = true;
                sldEconomy.Value = Strategy.EconomyWeight;
                sldEnvironment.Value = Strategy.EnvironmentWeight;
                sldWellbeing.Value = Strategy.WellbeingWeight;
                sldMoney.Value = Strategy.MoneyWeight;
                sldPower.Value = Strategy.PowerWeight;
                sldOil.Value = Strategy.OilWeight;
                sldHouses.Value = Strategy.HomesWeight;
                autoUpdatingSliders = false;
            });
        }   

        private void UpdateValues()
        {
            this.Strategy.EconomyWeight = sldEconomy.Value / SCALE;
            this.Strategy.EnvironmentWeight = sldEnvironment.Value / SCALE;
            this.Strategy.WellbeingWeight = sldWellbeing.Value / SCALE;
            this.Strategy.MoneyWeight = sldMoney.Value / SCALE;
            this.Strategy.OilWeight = sldOil.Value / SCALE;
            this.Strategy.PowerWeight = sldPower.Value / SCALE;
            this.Strategy.HomesWeight = sldHouses.Value / SCALE;

            this.Strategy.Normalize();
        }

        private async void UpdateCycle()
        {
            while (true)
            {
                UpdateValues();
                UpdateSliders();
                if (shouldCallUpdate)
                {
                    shouldCallUpdate = false;
                    if (Updated != null) Updated(this, new StrategyUpdatedEventArgs(strategy.Weights));
                }
                await Task.Delay(100);
            }
        }

        private void UpdateValues(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateValues();
        }

        private void StrategyUpdatedByUser(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!autoUpdatingSliders)
                shouldCallUpdate = true;
        }

      

        
    }
}
