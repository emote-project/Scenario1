using EmoteEvents;
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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WOZInterface.GameStrategy
{

    public class GameStrategyModel : ViewModelBase
    {
        private ObservableCollection<EnercitiesActionInfo> bestActions = new ObservableCollection<EnercitiesActionInfo>();
        public ObservableCollection<EnercitiesActionInfo> BestActions
        {
            get
            {
                return bestActions;
            }
            set
            {
                bestActions = value;
                NotifyPropertyChanged("BestActions");
            }
        }

        public GameStrategyModel() { }
        public void Reset()
        {
            BestActions = new ObservableCollection<EnercitiesActionInfo>();
        }

        private bool newActionNeeded = true;
        public bool NewActionNeeded
        {
            get
            {
                return newActionNeeded;
            }
            set
            {
                newActionNeeded = value;
                NotifyPropertyChanged("NewActionNeeded");
                UpdateButtonStatus();
            }
        }

        private bool canSendActionToGame = true;
        public bool CanSendActionToGame
        {
            get
            {
                return canSendActionToGame;
            }
            set
            {
                canSendActionToGame = value;
                NotifyPropertyChanged("CanSendActionToGame");
                UpdateButtonStatus();
            }
        }

        private bool isRecalculating = false;
        public bool IsRecalculating
        {
            get
            {
                return isRecalculating;
            }
            set
            {
                isRecalculating = value;
                NotifyPropertyChanged("IsRecalculating");
                UpdateButtonStatus();
            }
        }

        private bool playButtonActive = false;
        public bool PlayButtonActive
        {
            get
            {
                return playButtonActive; 
            }
            set
            {
                playButtonActive = value;
                NotifyPropertyChanged("PlayButtonActive");
            }
        }

        private bool recalculateButtonActive = true;
        public bool RecalculateButtonActive
        {
            get
            {
                return recalculateButtonActive; 
            }
            set
            {
                recalculateButtonActive = value;
                NotifyPropertyChanged("RecalculateButtonActive");
            }
        }

        private void UpdateButtonStatus()
        {
            PlayButtonActive = !isRecalculating && canSendActionToGame && BestActions.Count>0;
            RecalculateButtonActive = newActionNeeded && canSendActionToGame && !isRecalculating;
        }

    }

    public partial class GameStrategy : UserControl
    {
        GameStrategyClient client;
        GameStrategyModel data;

        Dictionary<EmoteEnercitiesMessages.EnercitiesRole, double[]> strategies = new Dictionary<EmoteEnercitiesMessages.EnercitiesRole, double[]>();

        public GameStrategy()
        {
            InitializeComponent();
            strEconomist.SetEditable(false);
            strEnvironmentalist.SetEditable(false);
            strMayor.Updated += strMayor_Updated;
            data = DataContext as GameStrategyModel;
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                client = new GameStrategyClient(this);
            }
        }

        void strMayor_Updated(object sender, Common.AIStrategy.StrategyUpdatedEventArgs e)
        {
            strategies[EmoteEnercitiesMessages.EnercitiesRole.Mayor] = e.Weights;
            data.NewActionNeeded = true;
        }


        public void StrategyUpdateByAI(Dictionary<EmoteEnercitiesMessages.EnercitiesRole, double[]> strategies)
        {
            Dispatcher.BeginInvoke((Action)delegate
            {
                strMayor.Strategy = new EmoteEvents.Strategy(strategies[EmoteEnercitiesMessages.EnercitiesRole.Mayor]);
                strEconomist.Strategy = new EmoteEvents.Strategy(strategies[EmoteEnercitiesMessages.EnercitiesRole.Economist]);
                strEnvironmentalist.Strategy = new EmoteEvents.Strategy(strategies[EmoteEnercitiesMessages.EnercitiesRole.Environmentalist]);
            });
        }

        public void RecalculateGameActions()
        {
            if (client != null)
            {
                data.NewActionNeeded = false;
                data.IsRecalculating = true;
                Dispatcher.BeginInvoke((Action)delegate {
                    data.BestActions.Clear();
                });
                StrategiesSet strategiesSet = new StrategiesSet(strategies);
                WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.UpdateStrategies(strategiesSet.SerializeToJson());
            }
            else
            {
                MessageBox.Show("Not connected to thalamus!");
            }
        }

        public void UpdateBestActionList(string[] EnercitiesActionInfo_actionInfos)
        {
            List<EnercitiesActionInfo> actionInfo = new List<EnercitiesActionInfo>();
            foreach (string ai in EnercitiesActionInfo_actionInfos)
            {
                if (ai != null)
                {
                    actionInfo.Add(EnercitiesActionInfo.DeserializeFromJson(ai));
                }
            }
            data.BestActions = new ObservableCollection<EnercitiesActionInfo>(actionInfo);
            data.IsRecalculating = false;
        }

        public void TurnChanged(EnercitiesGameInfo gameInfo)
        {
            data.BestActions = new ObservableCollection<EnercitiesActionInfo>();
            if (gameInfo.CurrentRole == EmoteEnercitiesMessages.EnercitiesRole.Mayor)
            {
                data.CanSendActionToGame = true;
                data.NewActionNeeded = true;
            }
            else
            {
                data.CanSendActionToGame = false;
                data.NewActionNeeded = false;
                RecalculateGameActions();
            }
        }

        public void Reset()
        {
            data.Reset();
        }

        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.SkipTurn();
        }

        private void btnRecalculate_Click(object sender, RoutedEventArgs e)
        {
            RecalculateGameActions();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (lstBestActions.Items.Count>0 && lstBestActions.SelectedIndex == -1)
            {
                lstBestActions.SelectedItem = data.BestActions[0];
            }
            EnercitiesActionInfo action = lstBestActions.SelectedItem as EnercitiesActionInfo;
            if (action != null)
            {
                switch (action.ActionType)
                {
                    case EmoteEnercitiesMessages.ActionType.BuildStructure:
                        EmoteEnercitiesMessages.StructureType structure = (EmoteEnercitiesMessages.StructureType)action.SubType;
                        WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.ConfirmConstruction(structure, action.CellX, action.CellY);
                        break;
                    case EmoteEnercitiesMessages.ActionType.ImplementPolicy:
                        EmoteEnercitiesMessages.PolicyType policy = (EmoteEnercitiesMessages.PolicyType)action.SubType;
                        WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.ImplementPolicy(policy);
                        break;
                    case EmoteEnercitiesMessages.ActionType.UpgradeStructure:
                        EmoteEnercitiesMessages.UpgradeType upgrade = (EmoteEnercitiesMessages.UpgradeType)action.SubType;
                        WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.PerformUpgrade(upgrade, action.CellX, action.CellY);
                        break;
                    case EmoteEnercitiesMessages.ActionType.SkipTurn:
                        WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.SkipTurn();
                        break;
                }
                data.BestActions.Remove(action);
            }
        }

        private void btnImitateEnv_Click(object sender, RoutedEventArgs e)
        {
            strMayor.Strategy = new Strategy(strEnvironmentalist.Strategy.Weights);
        }

        private void btnImitateEco_Click(object sender, RoutedEventArgs e)
        {
            strMayor.Strategy = new Strategy(strEconomist.Strategy.Weights);
        }

        private void btnPanic_Click(object sender, RoutedEventArgs e)
        {
            data.PlayButtonActive = true;
            data.RecalculateButtonActive = true;
        }


    }
}
