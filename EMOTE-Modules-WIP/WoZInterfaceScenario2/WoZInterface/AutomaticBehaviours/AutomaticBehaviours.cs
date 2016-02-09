using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOZInterface.AutomaticBehaviours
{
    class AutomaticBehavioursViewModel : ViewModelBase
    {
        public static string PATH = "";

        string[] waitingAnimations = new string[] { "rodinThinking", "supermanThinking", "thinking" };
        public string[] WaitingAnimations
        {
            get
            {
                return waitingAnimations;
            }
            set
            {
                waitingAnimations = value;
                NotifyPropertyChanged("WaitingAnimations");
            }
        }
        string[] playingAnimations = new string[] { "", "", "" };
        public string[] PlayingAnimations
        {
            get
            {
                return playingAnimations;
            }
            set
            {
                playingAnimations = value;
                NotifyPropertyChanged("PlayingAnimations");
            }
        }
        List<KeyValuePair<EmoteEnercitiesMessages.ActionType, string>> categoryNames = new List<KeyValuePair<EmoteEnercitiesMessages.ActionType, string>>() {
            new KeyValuePair<EmoteEnercitiesMessages.ActionType, string>(EmoteEnercitiesMessages.ActionType.BuildStructure,"ConfirmConstruction"),
            new KeyValuePair<EmoteEnercitiesMessages.ActionType, string>(EmoteEnercitiesMessages.ActionType.ImplementPolicy,"ImplementPolicy"),
            new KeyValuePair<EmoteEnercitiesMessages.ActionType, string>(EmoteEnercitiesMessages.ActionType.UpgradeStructure,"PerformUpgrade"),
        };
        public List<KeyValuePair<EmoteEnercitiesMessages.ActionType, string>> CategoryNames
        {
            get 
            {
                return categoryNames;
            }
            set
            {
                categoryNames = value;
                NotifyPropertyChanged("CategoryNames");
            }
        }
        
        public static AutomaticBehavioursViewModel Load()
        {
            if (File.Exists(PATH))
            {
                try
                {
                    using (FileStream f = new FileStream(PATH, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader s = new StreamReader(f))
                        {
                            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(AutomaticBehavioursViewModel));
                            return x.Deserialize(s) as AutomaticBehavioursViewModel;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Can't load settings from file " + PATH + ". Exception: " + e.Message);
                }
            }
            else
            {
                return new AutomaticBehavioursViewModel();
            }
        }

        public bool Save()
        {
            try
            {
                using (FileStream f = new FileStream(PATH, FileMode.CreateNew, FileAccess.Write))
                {
                    using (StreamWriter s = new StreamWriter(f))
                    {
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
                        x.Serialize(s, this);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Can't save settings to file " + PATH +". Exception: "+e.Message);
            }
            return true;
        }
    }

    class AutomaticBehaviours
    {
        private const double MAX_ANIM_CYCLE_DURATION = 5000;

        AutomaticBehavioursViewModel data;
        bool playingAnimation = false;
        string animationID = "";
        DateTime animationCycleStart = new DateTime();

        bool IsRecalculating = false;
        bool IsAITurn = false;

        public AutomaticBehaviours()
        {
            AutomaticBehavioursClient client = new AutomaticBehavioursClient(this, WoZOptimusPrimeMegaClient.GetInstance().CharacterName);
            data = AutomaticBehavioursViewModel.Load();
        }

        public void BestActionsPlannedEvent()
        {
            IsRecalculating = false;
        }

        public void StrategiesUpdatedEvent()
        {
            IsRecalculating = true;
            if (IsAITurn)
                ExecuteThinking();
        }

        public void AnimationFinishedEvent(string ID)
        {
            if (ID.Equals(animationID))
            {
                playingAnimation = false;
                animationID = "";
            }
        }

        public void TurnChangedEvent(EmoteEvents.EnercitiesGameInfo gameInfo)
        {
            IsAITurn = gameInfo.CurrentRole == EmoteEnercitiesMessages.EnercitiesRole.Mayor;
        }

        public void GameActionPlayed(EmoteEnercitiesMessages.ActionType type, int subtype, string translation)
        {
            string category = "";
            if (IsAITurn)
            {
                switch (type)
                {
                    case EmoteEnercitiesMessages.ActionType.BuildStructure:
                        category = data.CategoryNames.Find(x => x.Key==EmoteEnercitiesMessages.ActionType.BuildStructure).Value;            // DANGEROUS null possible
                        break;
                    case EmoteEnercitiesMessages.ActionType.ImplementPolicy:
                        category = data.CategoryNames.Find(x => x.Key == EmoteEnercitiesMessages.ActionType.ImplementPolicy).Value;         // DANGEROUS null possible
                        break;
                    case EmoteEnercitiesMessages.ActionType.UpgradeStructure:
                        category = data.CategoryNames.Find(x => x.Key == EmoteEnercitiesMessages.ActionType.UpgradeStructure).Value;        // DANGEROUS null possible
                        break;
                }
                WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.PerformUtteranceFromLibrary("", category, "Self", new string[0], new string[0]);
            }
        }

        public async void ExecuteThinking()
        { 
            animationCycleStart = DateTime.Now;
            double elapsedTime = 0;
            while (IsRecalculating && elapsedTime < MAX_ANIM_CYCLE_DURATION)
            {
                if (!playingAnimation)
                {
                    string selAnimation = data.WaitingAnimations[new Random().Next(data.WaitingAnimations.Length)];
                    animationID = selAnimation + (new DateTime().Ticks);
                    await Task.Delay(new Random().Next(3));
                    WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.PlayAnimation(animationID, selAnimation);
                    playingAnimation = true;
                }
                await Task.Delay(100);
                elapsedTime = DateTime.Now.Subtract(animationCycleStart).TotalMilliseconds;
            }
            playingAnimation = false;
        }

        public async void ExecutePlayingAction()
        {
            string category = "";
            string subcategory = "";
            WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.PerformUtteranceFromLibrary("",category,subcategory,new string[0], new string[0]);
        }

    }
}
