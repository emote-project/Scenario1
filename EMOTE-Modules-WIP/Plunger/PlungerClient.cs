using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Thalamus;
using ThalamusLogTool;
using EmoteEnercitiesMessages;
using EmoteEvents;
using EmoteCommonMessages;
using Thalamus.BML;

namespace Plunger
{
    public interface IPlunger
    {
    }

    public interface IPlungerPublisher : IThalamusPublisher, IEnercitiesTaskActions, IFMLSpeechEvents
    {
    }

    public class PlungerClient : ThalamusClient, IPlunger
    {
        public class PlungerPublisher : IPlungerPublisher
        {
            dynamic publisher;
            public PlungerPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }

            public void ConfirmConstruction(StructureType structure, int cellX, int cellY)
            {
                publisher.ConfirmConstruction(structure, cellX, cellY);
            }

            public void ImplementPolicy(PolicyType policy)
            {
                publisher.ImplementPolicy(policy);
            }

            public void PerformUpgrade(UpgradeType upgrade, int cellX, int cellY)
            {
                publisher.PerformUpgrade(upgrade, cellX, cellY);
            }

            public void PlayStrategy(EnercitiesStrategy strategy)
            {
                publisher.PlayStrategy(strategy);
            }

            public void SkipTurn()
            {
                publisher.SkipTurn();
            }

            public void UtteranceFinished(string id)
            {
                publisher.UtteranceFinished(id);
            }

            public void UtteranceStarted(string id)
            {
                publisher.UtteranceStarted(id);
            }
        }

        public List<String> Messages { get; set; }
        public MonitoringSettings MonitorSettings;

        public Dictionary<string, DateTime> MessagesTime = new Dictionary<string, DateTime>();
        public Dictionary<string, PML> LastMessages = new Dictionary<string, PML>();
        private Dictionary<string, Type> LoadedTypes = new Dictionary<string, Type>();

        public List<String> AllMessages { get; set; }

        public PlungerPublisher PlungePublisher;

        public string CharacterName = "";

        public PlungerClient(string characterName = "")
            : base("Desentupidor", characterName, false) 
        {
            CharacterName = characterName;
            AllMessages = new List<string>();
            Messages = new List<string>();
            if (Properties.Settings.Default.MonitoringSettingsFile != "") MonitorSettings = MonitoringSettings.Load(Properties.Settings.Default.MonitoringSettingsFile);
            else MonitorSettings = new MonitoringSettings();
            LoadInterfaces();
            /*foreach (string msg in Messages)
            {
                if (!LoadedTypes.ContainsKey(msg.Split('.')[0]))
                {
                    Messages.Remove(msg);
                }
            }*/
            SetPublisher<IPlungerPublisher>();
            PlungePublisher = new PlungerPublisher(Publisher);

            Start();
        }

        public override void ConnectedToMaster()
        {
            RegisterExtraInterface(typeof(IFMLSpeechEvents), this);
            RegisterExtendedInterface("IFMLSpeechEvents");

            RegisterExtraInterface(typeof(IEnercitiesGameStateEvents), this);
            RegisterExtendedInterface("IEnercitiesGameStateEvents");

            RegisterExtraInterface(typeof(ISpeakActions), this);
            RegisterExtendedInterface("ISpeakActions");

            foreach (string s in base.AllEventsInfo.Keys) if (!AllMessages.Contains(s)) AllMessages.Add(s);
            RegisterMessages(MonitorSettings.Messages);
        }

        public override void ReceiveEventInfo(string[] piNames, string[][] piParameters, string[][] piTypes)
        {
            base.ReceiveEventInfo(piNames, piParameters, piTypes);
            foreach (string s in base.AllEventsInfo.Keys) if (!AllMessages.Contains(s)) AllMessages.Add(s);
        }

        public void RegisterMessages(List<string> newMessages)
        {
            List<string> toRemove = new List<string>();
            foreach (string msg in Messages)
            {
                if (!newMessages.Contains(msg)) toRemove.Add(msg);
            }
            foreach (string s in toRemove) Messages.Remove(s);


            foreach (string msg in newMessages)
            {
                string i = msg.Split('.')[0];
                if (LoadedTypes.ContainsKey(i) && !Messages.Contains(msg))
                {
                    Messages.Add(msg);
                    base.RegisterExtraInterface(LoadedTypes[i], this);
                    base.RegisterExtendedInterface(i);
                }
                else
                {
                    DebugError("Unable to monitor message {0}: interface {1} was not found. Please place the required assemply (dll/exe) in the Plunger's output directory.", msg, i);
                }
            }
        }

        public delegate void EnercitiesGameStateTurnChangedHandler(EnercitiesGameInfo egi);
        public event EnercitiesGameStateTurnChangedHandler EnercitiesGameStateTurnChanged;
        public void NotifyEnercitiesGameStateTurnChanged(EnercitiesGameInfo egi)
        {
            if (EnercitiesGameStateTurnChanged != null) EnercitiesGameStateTurnChanged(egi);
        }

        public delegate void UtteranceStartedHandler(string id);
        public event UtteranceStartedHandler UtteranceStarted;
        public void NotifyUtteranceStarted(string id)
        {
            if (UtteranceStarted != null) UtteranceStarted(id);
        }

        public delegate void UtteranceFinishedHandler(string id);
        public event UtteranceFinishedHandler UtteranceFinished;
        public void NotifyUtteranceFinished(string id)
        {
            if (UtteranceFinished != null) UtteranceFinished(id);
        }

        public delegate void SpeakActionHandler(string id, string text);
        public event SpeakActionHandler SpeakAction;
        public void NotifySpeakAction(string id, string text)
        {
            if (SpeakAction != null) SpeakAction(id, text);
        }

        public delegate void EnercitiesGameStateHandler(string state);
        public event EnercitiesGameStateHandler EnercitiesGameState;
        public void NotifyEnercitiesGameState(string state)
        {
            if (EnercitiesGameState != null) EnercitiesGameState(state);
        }

        protected override void ReceiveExtendedInterfaceMessage(PML p)
        {
            if (Messages.Contains(p.Name))
            {
                MessagesTime[p.Name] = DateTime.Now;
                LastMessages[p.Name] = p;
            }

            switch(p.Name) 
            {
                case "ISpeakActions.Speak": NotifySpeakAction(p.Parameters["id"].GetValue(), p.Parameters["text"].GetValue());
                    break;
                case "ISpeakActions.SpeakBookmarks": 
                    string fullText = "";
                    foreach (string s in p.Parameters["text"].GetStringArray()) fullText += s + " ";
                    NotifySpeakAction(p.Parameters["id"].GetValue(), fullText);
                    break;
                case "IFMLSpeechEvents.UtteranceFinished": NotifyUtteranceFinished(p.Parameters["id"].GetValue());
                    break;
                case "IFMLSpeechEvents.UtteranceStarted": NotifyUtteranceStarted(p.Parameters["id"].GetValue());
                    break;
                
                case "IEnercitiesGameStateEvents.EndGameNoOil": NotifyEnercitiesGameState("EndGameNoOil");
                    break;
                case "IEnercitiesGameStateEvents.EndGameSuccessfull": NotifyEnercitiesGameState("EndGameSuccessfull");
                    break;
                case "IEnercitiesGameStateEvents.EndGameTimeOut": NotifyEnercitiesGameState("EndGameTimeOut");
                    break;
                case "IEnercitiesGameStateEvents.GameStarted": NotifyEnercitiesGameState("GameStarted");
                    break;

                case "IEnercitiesGameStateEvents.ResumeGame": 
                case "IEnercitiesGameStateEvents.TurnChanged": NotifyEnercitiesGameStateTurnChanged(EnercitiesGameInfo.DeserializeFromJson(p.Parameters["serializedGameState"].GetString()));
                    break;
            }
        }

        private void LoadInterfaces()
        {
            Debug("Searching for interfaces in directory {0}", Directory.GetCurrentDirectory());
            //Dictionary<string, Type> from = LogTool.LoadInterfaces(Directory.GetCurrentDirectory());

            string assembliesPath = Directory.GetCurrentDirectory();
            List<Assembly> allAssemblies = new List<Assembly>();
            foreach (string ass in Directory.GetFiles(assembliesPath, "*.dll"))
            {
                try
                {
                    allAssemblies.Add(Assembly.LoadFile(ass));
                    Debug("Found assembly '{0}'", ass);
                }
                catch (Exception e)
                {
                    DebugException(e);
                }
            }
            foreach (string ass in Directory.GetFiles(assembliesPath, "*.exe"))
            {
                try
                {
                    allAssemblies.Add(Assembly.LoadFile(ass));
                    Debug("Found assembly '{0}'", ass);
                }
                catch (Exception e)
                {
                    DebugException(e);
                }
            }


            foreach (Assembly a in allAssemblies)
            {
                try
                {
                    foreach (Type t in a.GetTypes())
                    {
                        if (ThalamusClient.ImplementsTypeDirectly(t, typeof(IAction)))
                        {
                            LoadedTypes[t.Name] = t;
                        }
                        else if (ThalamusClient.ImplementsTypeDirectly(t, typeof(IPerception)))
                        {
                            LoadedTypes[t.Name] = t;
                        }
                    }
                    PML.AddAssembly(a);
                }
                catch (System.Reflection.ReflectionTypeLoadException re)
                {
                    Console.WriteLine("Can't load types in assembly: " + a.FullName +
                        System.Environment.NewLine + "\t" + re.Message +
                        System.Environment.NewLine + "\t" + re.LoaderExceptions);
                }
            }
        }

    }
}
