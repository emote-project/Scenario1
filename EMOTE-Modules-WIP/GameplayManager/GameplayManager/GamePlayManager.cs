
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmoteEvents;
using Thalamus;
using EmoteEnercitiesMessages;
using EmoteCommonMessages;
using System.Threading;
using System.Reflection;
using System.IO;
using Skene;
using System.Threading.Tasks;
using Skene.Utterances;

namespace GameplayManager
{
    public interface IFMLSpeechAux : IAction
    {
        void ReplaceTagsAndPerform(string utterance, string category);
    }

    public interface IGameplayManager : 
        IEnercitiesExamineEvents, 
        IEnercitiesTaskEvents,
        IEnercitiesGameStateEvents,
        ITargetEvents,
        IFMLSpeechAux,
        Thalamus.BML.ISpeakEvents,
        IEnercitiesAIActions
    {
    }

    public interface IGameplayManagerPublisher :
        IFMLSpeech,
        IEnercitiesTaskActions,
        ITargetEvents,
        IEnercitiesAIPerceptions
    { }


    public class GamePlayManager : ThalamusClient, IGameplayManager
    {
        
        public class XYInt {
            public int X { get; set; }
            public int Y { get; set; }
            public XYInt(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
        
        public IGameplayManagerPublisher GamePublisher;
        private EnercitiesGamestate gameState;
        private UtteranceLibrary utteranceLibrary;
        private bool enabled = true;

        private Gender Player1Gender;
        private Gender Player2Gender;

        private List<EnercitiesActionInfo> bestActionsForThisTurn;
        private bool isBestActionUpdated = false;

        private bool playAllRoles = false;

        /*Proposal variables*/
        private int proposalHouses = 0;
        private int proposalPolicies = 0;

        private int numTurnsInLevel = 0;

        private Boolean endOfLevel = false;

        private class GameplayManagerPublisher : IGameplayManagerPublisher
        {
            dynamic publisher;

            public GameplayManagerPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }


            #region IFMLSpeech Members

            public void PerformUtterance(string id, string utterance, string category)
            {
                publisher.PerformUtterance(id, utterance, category);
            }

            public void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues)
            {
                publisher.PerformUtteranceFromLibrary(id, category, subcategory, tagNames, tagValues);
            }

            public void CancelUtterance(string id)
            {
                publisher.CancelUtterance(id);
            }

            #endregion

            #region IEnercitiesTaskActions Members

            public void PlayStrategy(EnercitiesStrategy strategy)
            {
                publisher.PlayStrategy(strategy);
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

            public void SkipTurn()
            {
                publisher.SkipTurn();
            }

            #endregion

            #region ITargetEvents Members

            public void TargetScreenInfo(string targetName, int X, int Y)
            {
                publisher.TargetScreenInfo(targetName, X, Y);
            }

            public void TargetLink(string targetName, string linkedTargetName)
            {
                publisher.TargetLink(targetName, linkedTargetName);
            }

            #endregion

            public void UpdateStrategies(string StrategiesSet_strategies)
            {
                publisher.UpdateStrategies(StrategiesSet_strategies);
            }
        }


        private CollaborativeStrategySelector.StrategySelector SupportRoleSelector;

        private int utterancesCount = 0;

        private Dictionary<string, XYInt> SavedTargets;

        public GamePlayManager(string character, string state, bool playAllRoles = false)      // If state != enabled => the client will ignore all the messages excluding those in IGamePlayManagerRequests
            : base("GamePlayManager", character)
        {
            SavedTargets = new Dictionary<string,XYInt>();
            SetPublisher<IGameplayManagerPublisher>();
            GamePublisher = new GameplayManagerPublisher(Publisher);
            Debug("Current directory: " + Directory.GetCurrentDirectory());
            utteranceLibrary = new UtteranceLibrary();
            utteranceLibrary.LoadLibrary(@"..\..\Utterances_NAO.xlsx");
            SupportRoleSelector = new CollaborativeStrategySelector.StrategySelector(@"..\..\Tabela.xlsx");
            enabled = state.Equals("enabled");
            this.playAllRoles = playAllRoles;
        }

        private string UtId()
        {
            return "GamePlayUtterance_" + utterancesCount++;
        }

        private string GetUtterance(string category, string subcategory = "-")
        {
            //string subcategory;

            //if (noSubcategory) subcategory = "-";
            //else
            //{
            //    if (gameStatus.currentPlayerType == GameStatus.CurrentPlayerType.AI) subcategory = Commons.SUBCAT_SELF;
            //    else subcategory = Commons.SUBCAT_OTHER;
            //}
            if (gameState == null || !enabled) return "";
            string utt = utteranceLibrary.GetUtterance(category, subcategory);
            utt = ReplaceTags(utt);
            return utt;

        }

        private string ReplaceTags(string utt)
        {
            if (utt != "" && utt != null)
            {
                utt = utt.Replace(Commons.TAG_ENVIRONMENT_SCORE, gameState.EnercitiesInfo.EnvironmentScore + "");
                utt = utt.Replace(Commons.TAG_ECONOMY_SCORE, gameState.EnercitiesInfo.EconomyScore + "");
                utt = utt.Replace(Commons.TAG_MAYOR_SCORE, gameState.EnercitiesInfo.WellbeingScore + "");
                utt = utt.Replace(Commons.TAG_CURRENT_MONEY, gameState.EnercitiesInfo.Money + "");
                utt = utt.Replace(Commons.TAG_CURRENT_OIL, gameState.EnercitiesInfo.Oil + "");
                utt = utt.Replace(Commons.TAG_CURRENT_ENERGY, gameState.EnercitiesInfo.PowerProduction + "");
                utt = utt.Replace(Commons.TAG_STRUCTURE_CATEGORY, gameState.LastStructureCategory.ToString() + "");

                utt = utt.Replace(Commons.TAG_ARTIGO_DEFINIDO_ECO, (gameState.Economist == null ? "o" : (gameState.Economist.Gender == Gender.Male ? "o" : "a")));
                utt = utt.Replace(Commons.TAG_ARTIGO_DEFINIDO_ENV, (gameState.Environmentalist == null ? "o" : (gameState.Environmentalist.Gender == Gender.Male ? "o" : "a")));
                utt = utt.Replace(Commons.TAG_ARTIGO_DEFINIDO_CURR_PLAYER, (gameState.CurrentPlayer == null ? "o" : (gameState.CurrentPlayer.Gender == Gender.Male ? "o" : "a")));
                if (gameState.CurrentPlayer == null)
                {
                    utt = utt.Replace(Commons.TAG_ARTIGO_DEFINIDO_NEXT_PLAYER, (gameState.CurrentPlayer == null ? "o" : (gameState.CurrentPlayer.Gender == Gender.Male ? "o" : "a")));
                }
                else
                {
                    Player nextPlayer = null;
                    if (gameState.CurrentPlayer == gameState.Environmentalist) nextPlayer = gameState.Economist;
                    else if (gameState.CurrentPlayer == gameState.Economist) nextPlayer = null;
                    else nextPlayer = gameState.Environmentalist;

                    utt = utt.Replace(Commons.TAG_ARTIGO_DEFINIDO_NEXT_PLAYER, (nextPlayer == null ? "o" : (nextPlayer.Gender == Gender.Male ? "o" : "a")));
                }
                
                utt = utt.Replace(Commons.TAG_CURRENT_PLAYER_NAME, gameState.CurrentPlayer == null ? "" : gameState.CurrentPlayer.Name);
                utt = utt.Replace(Commons.TAG_CURRENT_LEVEL, gameState.EnercitiesInfo.Level + "");
                utt = utt.Replace(Commons.TAG_LEVEL, gameState.EnercitiesInfo.Level + "");

                utt = utt.Replace(Commons.TAG_CURRENT_PLAYER_SIDE, gameState.GetCurrentPlayerSide());
                utt = utt.Replace(Commons.TAG_ECO_NAME, gameState.Economist == null ? "" : gameState.Economist.Name);
                utt = utt.Replace(Commons.TAG_ENV_NAME, gameState.Environmentalist == null ? "" : gameState.Environmentalist.Name);
                utt = utt.Replace(Commons.TAG_POLICY, ClearUnderscores(gameState.LastPolicyTranslation));
                utt = utt.Replace(Commons.TAG_STRUCTURE, ClearUnderscores(gameState.LastStructureTranslation));
                utt = utt.Replace(Commons.TAG_UPGRADE, ClearUnderscores(gameState.LastUpgradeTranslation));
                utt = utt.Replace(Commons.TAG_TOTAL_SCORE, gameState.EnercitiesInfo.GlobalScore + "");
                utt = utt.Replace(Commons.TAG_CURRENT_PLAYER_ROLE, gameState.CurrentPlayer.Role.ToString());
                switch (gameState.EnercitiesInfo.CurrentRole)
                {
                    case EnercitiesRole.Economist:
                        utt = utt.Replace(Commons.TAG_CURRENT_PLAYER_ROLE_SPEECH, "Economista");
                        break;
                    case EnercitiesRole.Environmentalist:
                        utt = utt.Replace(Commons.TAG_CURRENT_PLAYER_ROLE_SPEECH, "Ambientalista");
                        break;
                    case EnercitiesRole.Mayor:
                        utt = utt.Replace(Commons.TAG_CURRENT_PLAYER_ROLE_SPEECH, "Presidente");
                        break;
                    default:
                        break;
                }
                
                utt = utt.Replace(Commons.TAG_STRUCTURE_CATEGORY, ClearUnderscores(gameState.LastStructureCategory.ToString()));
            }
            return utt;
        }

        public void TurnChanged(string serializedGameState)
        {
            if (gameState == null) return;
            isBestActionUpdated = false;
            gameState.Update(EnercitiesGameInfo.DeserializeFromJson(serializedGameState));
            gameState.NumberOfUpgradesDoneThisTurn = 0;
            numTurnsInLevel++;

            Debug("TurnChanged: " + gameState.ToString());
            if (!enabled) return;
            Thread.Sleep(1000);
            if (!endOfLevel)
            {
                if (gameState.CurrentPlayer.IsAI())
                {
                    GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_TURN_CHANGED, Commons.SUBCAT_SELF), Commons.CAT_TURN_CHANGED + "." + Commons.SUBCAT_SELF);
                }
                else
                {
                    GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_TURN_CHANGED, Commons.SUBCAT_OTHER), Commons.CAT_TURN_CHANGED + "." + Commons.SUBCAT_OTHER);
                }
            }
        }

        private async void Play()
        {
            EnercitiesActionInfo action = bestActionsForThisTurn[0];
            if (bestActionsForThisTurn != null)
            {
                switch (action.ActionType)
                {
                    case EmoteEnercitiesMessages.ActionType.BuildStructure:
                        EmoteEnercitiesMessages.StructureType structure = (EmoteEnercitiesMessages.StructureType)action.SubType;
                        GamePublisher.ConfirmConstruction(structure, action.CellX, action.CellY);
                        break;
                    case EmoteEnercitiesMessages.ActionType.ImplementPolicy:
                        EmoteEnercitiesMessages.PolicyType policy = (EmoteEnercitiesMessages.PolicyType)action.SubType;
                        GamePublisher.ImplementPolicy(policy);
                        break;
                    case EmoteEnercitiesMessages.ActionType.UpgradeStructure:
                        EmoteEnercitiesMessages.UpgradeType upgrade = (EmoteEnercitiesMessages.UpgradeType)bestActionsForThisTurn[0].SubType;
                        GamePublisher.PerformUpgrade(upgrade, action.CellX, action.CellY);
                        
                        Console.WriteLine("\n\nUpgrade 1");
                        await WaitForUpdate(1);
                        if (bestActionsForThisTurn.Count >= 2 && bestActionsForThisTurn[2] != null && bestActionsForThisTurn[1].ActionType == ActionType.UpgradeStructure)  // It may happen that the economist plays his turn quicker than the AI takes to calculate the best actions for him. In this case the list of best actions will be received during the AI turn. If this happens, this code will be executed a first time with those values and than a second time with the new best actions calculated for this turn. Since this is a quick code made just to demo the project, I'm just making sure that it doesn't crash.
                        {
                            upgrade = (EmoteEnercitiesMessages.UpgradeType)bestActionsForThisTurn[1].SubType;
                            GamePublisher.PerformUpgrade(upgrade, bestActionsForThisTurn[1].CellX, bestActionsForThisTurn[1].CellY);
                            Console.WriteLine("\n\nUpgrade 2");
                            await WaitForUpdate(2);
                            if (bestActionsForThisTurn.Count >= 3 && bestActionsForThisTurn[2]!=null && bestActionsForThisTurn[2].ActionType == ActionType.UpgradeStructure)
                            {
                                upgrade = (EmoteEnercitiesMessages.UpgradeType)bestActionsForThisTurn[2].SubType;
                                GamePublisher.PerformUpgrade(upgrade, bestActionsForThisTurn[2].CellX, bestActionsForThisTurn[2].CellY);
                                Console.WriteLine("\n\nUpgrade 3");
                            }
                        }
                        else
                        {
                            GamePublisher.SkipTurn();
                        }
                        

                        break;
                    case EmoteEnercitiesMessages.ActionType.SkipTurn:
                        GamePublisher.SkipTurn();
                        break;
                }
            }
        }

        public async void BestActionPlanned(string[] EnercitiesActionInfo_actionInfos)
        {
            bestActionsForThisTurn = new List<EnercitiesActionInfo>();
            foreach (string a in EnercitiesActionInfo_actionInfos)
            {
                bestActionsForThisTurn.Add(EnercitiesActionInfo.DeserializeFromJson(a));
            }
            isBestActionUpdated = true;
            await GameStartPresentationEnded();
            if (enabled && (gameState.CurrentPlayer.IsAI() || playAllRoles))
            {
                Play();
            }
        }

        public void BestActionsPlanned(string[] EnercitiesActionInfo_actionInfos)
        {
            
        }

        private async Task<int> GameStartPresentationEnded()
        {
            while (!gameState.GameStartPresentationEnded)
            {
                await Task.Delay(500);
            }
            return 0;
        }

        private async Task<int> WaitForUpdate(int updateNumber)
        {
            while (gameState.NumberOfUpgradesDoneThisTurn != updateNumber)
            {
                await Task.Delay(500);
            }
            return 0;
        }

        public void StrategiesUpdated(string StrategiesSet_strategies)
        {
            GamePublisher.UpdateStrategies(StrategiesSet_strategies);
        }

        void IFMLSpeechAux.ReplaceTagsAndPerform(string utterance, string category)
        {
            if (gameState == null)
            {
                GamePublisher.PerformUtterance("", utterance, category);
                Console.WriteLine("WARNING: no game state. Did you started the game after starting this client?");
            }
            else
            {
                GamePublisher.PerformUtterance("", ReplaceTags(utterance), category);
            }
        }


        void IEnercitiesExamineEvents.BuildMenuTooltipClosed(StructureCategory category,string translation)
        {
            if (gameState == null) return;
            gameState.LastStructureCategory = category;
            gameState.LastStructureCategoryTranslation = translation;
            Debug("BuildMenuTooltipClosed: " + category +" ("+translation+")");
        }

        void IEnercitiesExamineEvents.BuildMenuTooltipShowed(StructureCategory category, string translation)
        {
            if (gameState == null) return;
            gameState.LastStructureCategory = category;
            gameState.LastStructureCategoryTranslation = translation;
            Debug("BuildMenuTooltipShowed: " + category+" ("+translation+")");
        }

        void IEnercitiesExamineEvents.BuildingMenuToolSelected(StructureType structure, string translation)
        {
            if (gameState == null) return;
            gameState.LastStructure = structure;
            gameState.LastStructureTranslation = translation;
            Debug("BuildingMenuToolSelected: " + structure + " ("+translation+")");
        }

        void IEnercitiesExamineEvents.BuildingMenuToolUnselected(StructureType structure, string translation)
        {
            if (gameState == null) return;
            gameState.LastStructure = structure;
            gameState.LastStructureTranslation = translation;
            Debug("BuildingMenuToolUnselected: " + structure+" ("+translation+")");
        }

        void IEnercitiesExamineEvents.PoliciesMenuClosed()
        {
            Debug("PoliciesMenuClosed");
        }

        void IEnercitiesExamineEvents.PoliciesMenuShowed()
        {
            Debug("PoliciesMenuShowed");
        }

        void IEnercitiesExamineEvents.PolicyTooltipClosed()
        {
            Debug("PolicyTooltipClosed");
        }

        void IEnercitiesExamineEvents.PolicyTooltipShowed(PolicyType policy, string translation)
        {
            if (gameState == null) return;
            Debug("PolicyTooltipShowed: " + policy+ " ("+translation+")");
            gameState.LastPolicy = policy;
            gameState.LastPolicyTranslation = translation;
        }

        void IEnercitiesExamineEvents.UpgradeTooltipClosed()
        {
            Debug("UpgradeTooltipClosed");
            if (gameState == null || !enabled) return;
        }

        void IEnercitiesExamineEvents.UpgradeTooltipShowed(UpgradeType upgrade, string translation)
        {
            if (gameState == null) return;
            gameState.LastUpgrade = upgrade;
            gameState.LastUpgradeTranslation = translation;
            Debug("UpgradeTooltipShowed: " + upgrade+" ("+translation+")");
        }

        void IEnercitiesExamineEvents.UpgradesMenuClosed()
        {
            if (gameState == null || !enabled) return;
            Debug("UpgradesMenuClosed");
        }

        void IEnercitiesExamineEvents.UpgradesMenuShowed()
        {
            Debug("UpgradesMenuShowed");
            if (gameState == null || !enabled) return;
            if (!gameState.CurrentPlayer.IsAI())
                GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_STRATEGY_PROPOSAL, Commons.SUBCAT_UPGRADE), Commons.CAT_STRATEGY_PROPOSAL + "." + Commons.SUBCAT_UPGRADE);
        }

        void IEnercitiesTaskEvents.ConfirmConstruction(StructureType structure, string translation, int cellX, int cellY)
        {
            if (gameState == null) return;
            gameState.LastStructure = structure;
            gameState.LastStructureTranslation = translation;
            if (gameState.LastStructureCategory.Equals(2))
                proposalHouses++; //counts the number of residential already constructed
            Debug("ConfirmConstruction: " + structure+" ("+cellX+","+cellY+")");
            if (!enabled) return;
            if (gameState.CurrentPlayer.IsAI())
            {
                GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_CONFIRM_CONSTRUCTION, Commons.SUBCAT_SELF), Commons.CAT_CONFIRM_CONSTRUCTION + "." + Commons.SUBCAT_SELF);
            }
            else
            {
                GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_CONFIRM_CONSTRUCTION, Commons.SUBCAT_OTHER), Commons.CAT_CONFIRM_CONSTRUCTION + "." + Commons.SUBCAT_OTHER);
            }
        }

        void IEnercitiesTaskEvents.ImplementPolicy(PolicyType policy, string translation)
        {
            if (gameState == null) return;
            gameState.LastPolicy = policy;
            gameState.LastPolicyTranslation = translation;
            proposalPolicies++; //counts the number of policies already implemented

            string category = MethodInfo.GetCurrentMethod().Name;
            Debug(category + ": " + policy);
            if (!enabled) return;
            GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_IMPLEMENT_POLICY, (gameState.CurrentPlayer.IsAI() ? Commons.SUBCAT_SELF : Commons.SUBCAT_OTHER)), Commons.CAT_IMPLEMENT_POLICY + "." + (gameState.CurrentPlayer.IsAI() ? Commons.SUBCAT_SELF : Commons.SUBCAT_OTHER));
        }

        void IEnercitiesTaskEvents.PerformUpgrade(UpgradeType upgrade, string translation, int cellX, int cellY)
        {
            if (gameState == null) return;
            gameState.NumberOfUpgradesDoneThisTurn++;
            gameState.LastUpgrade = upgrade;
            gameState.LastUpgradeTranslation = translation;

            string category = MethodInfo.GetCurrentMethod().Name;
            Debug(category + ": " + upgrade);
            Console.WriteLine("Upgrade this turn: " + gameState.NumberOfUpgradesDoneThisTurn);
            if (!enabled) return;
            GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_PERFORM_UPGRADE, (gameState.CurrentPlayer.IsAI() ? Commons.SUBCAT_SELF : Commons.SUBCAT_OTHER)), Commons.CAT_PERFORM_UPGRADE + "." + (gameState.CurrentPlayer.IsAI() ? Commons.SUBCAT_SELF : Commons.SUBCAT_OTHER));
        }

        void IEnercitiesTaskEvents.SkipTurn()
        {
            Debug("SkipTurn");
            if (gameState == null || !enabled) return;
            GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_SKIP_TURN), Commons.CAT_SKIP_TURN);
        }

        void IEnercitiesGameStateEvents.EndGameNoOil(int totalScore)
        {
            Debug("EndGameNoOil: "+totalScore);
            if (gameState == null || !enabled) return;
            GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_END_GAME, Commons.SUBCAT_NO_OIL), Commons.CAT_END_GAME + "." + Commons.SUBCAT_NO_OIL);
        }

        void IEnercitiesGameStateEvents.EndGameSuccessfull(int totalScore)
        {
            Debug("EndGameSuccessfull: "+totalScore);
            if (gameState == null || !enabled) return;
            GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_END_GAME, Commons.SUBCAT_SUCCESSFULL), Commons.CAT_END_GAME + "." + Commons.SUBCAT_SUCCESSFULL);
        }

        void IEnercitiesGameStateEvents.EndGameTimeOut(int totalScore)
        {
            Debug("EndGameTimeOut: "+totalScore);
            if (gameState == null || !enabled) return;
            GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_END_GAME, Commons.SUBCAT_TIMEOUT), Commons.CAT_END_GAME + "." + Commons.SUBCAT_TIMEOUT);
        }

        void IEnercitiesGameStateEvents.ReachedNewLevel(int level)
        {
            Debug("ReachedNewLevel");
            proposalHouses = 0;
            numTurnsInLevel = 0;
            if (gameState == null || !enabled) return;
            GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_REACHED_NEW_LEVEL), Commons.CAT_REACHED_NEW_LEVEL);
        }

        void IEnercitiesGameStateEvents.TurnChanged(string serializedGameState)
        {
            TurnChanged(serializedGameState);
        }

        void IEnercitiesGameStateEvents.GameStarted(string player1name, string player1role, string player2name, string player2role)
        {
            Debug("GameStarted: " + player1name + " (" + player1role + ") - " + player2name + " (" + player2role+")");
            if (!Enum.IsDefined(typeof(EnercitiesRole), player1role)) DebugError("Enum.EnercitiesRole '{0}' is not defined!", player1role);
            else if (!Enum.IsDefined(typeof(EnercitiesRole), player2role)) DebugError("Enum.EnercitiesRole '{0}' is not defined!", player2role);
            else
            {
                gameState = new EnercitiesGamestate(this, player1name, (EnercitiesRole)Enum.Parse(typeof(EnercitiesRole), player1role), player2name, (EnercitiesRole)Enum.Parse(typeof(EnercitiesRole), player2role));
                gameState.Player1.Gender = this.Player1Gender;
                gameState.Player2.Gender = this.Player2Gender;
                if (!enabled)
                {
                    gameState.GameStartPresentationEnded = true;
                    return;
                }
                gameState.GameStartPresentationEnded = false;
                gameState.IsDoingGameStartPresentation = true;
                GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_GAME_STARTED), Commons.CAT_GAME_STARTED);
            }
        }

        void Thalamus.BML.ISpeakEvents.SpeakFinished(string id)
        {
            if (gameState != null && gameState.IsDoingGameStartPresentation)
            {
                gameState.GameStartPresentationEnded = true;
            }
        }

        private string ClearUnderscores(string name)
        {
            return name.ToString().Replace('_', ' ');
        }


        void IEnercitiesGameStateEvents.ResumeGame(string player1name, string player1role, string player2name, string player2role, string serializedGameState)
        {
            gameState = new EnercitiesGamestate(this, player1name, (EnercitiesRole)Enum.Parse(typeof(EnercitiesRole), player1role), player2name, (EnercitiesRole)Enum.Parse(typeof(EnercitiesRole), player2role));
            gameState.Player1.Gender = this.Player1Gender;
            gameState.Player2.Gender = this.Player2Gender;
            gameState.GameStartPresentationEnded = true;
            TurnChanged(serializedGameState);
        }

        bool Condition1 = false;

        void IEnercitiesGameStateEvents.StrategyGameMoves(string environmentalistMove, string economistMove, string mayorMove, string globalMove)
        {
            /*alterar aqui: criar uma funçao de proposal!*/
            if (enabled)
            {
                if (Condition1)
                {
                    EmoteEnercitiesMessages.EnercitiesStrategy strategyDecision = SupportRoleSelector.GetStrategy(gameState.EnercitiesInfo);
                    if (gameState.CurrentPlayer.IsAI())
                    {
                        if (strategyDecision == EnercitiesStrategy.Economist)
                            GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_STRATEGY_CONFIRMATION, Commons.SUBCAT_ECONOMIST), Commons.CAT_STRATEGY_CONFIRMATION + "." + Commons.SUBCAT_ECONOMIST);
                        if (strategyDecision == EnercitiesStrategy.Environmentalist)
                            GamePublisher.PerformUtterance("", GetUtterance(Commons.SUBCAT_ENVIRONMENTALIST, Commons.SUBCAT_ENVIRONMENTALIST), Commons.CAT_STRATEGY_CONFIRMATION + "." + Commons.SUBCAT_ENVIRONMENTALIST);
                        GamePublisher.PlayStrategy(strategyDecision);
                    }
                    else
                    {// if it isn't the agent turn to play,it can suggest a proposal or just comment the game situation according to the Collaborative Evaluation utterances
                        // we could use a random number to define the type of play (INCOMPLETE)

                        PlayProposal(strategyDecision);
                        //playCollaborativeEvaluation();
                    }
                }
                else
                {
                    if (gameState.CurrentPlayer.IsAI())
                    {
                        GamePublisher.PlayStrategy(EnercitiesStrategy.Mayor);
                    }
                }
            }
        }

       
        void PlayProposal(EmoteEnercitiesMessages.EnercitiesStrategy chosenStrategy) 
        {
            Console.WriteLine("proposta: " + chosenStrategy);
            if (chosenStrategy == EnercitiesStrategy.Economist)
                GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_STRATEGY_PROPOSAL, Commons.SUBCAT_ECONOMIST), Commons.CAT_STRATEGY_PROPOSAL + "." + Commons.SUBCAT_ECONOMIST);
            else if (chosenStrategy == EnercitiesStrategy.Environmentalist)
                GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_STRATEGY_PROPOSAL, Commons.SUBCAT_ENVIRONMENTALIST), Commons.CAT_STRATEGY_PROPOSAL + "." + Commons.SUBCAT_ENVIRONMENTALIST);
            else if (chosenStrategy == EnercitiesStrategy.Global)
            {
                if (gameState.EnercitiesInfo.PowerProduction - gameState.EnercitiesInfo.PowerConsumption <= 2) //if energy has a low value
                    GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_STRATEGY_PROPOSAL_GLOBAL, Commons.SUBCAT_ENERGY), Commons.CAT_STRATEGY_PROPOSAL_GLOBAL + "." + Commons.SUBCAT_ENERGY);
                else if (gameState.EnercitiesInfo.Level == 1)
                {
                    if (gameState.LastStructureCategory.Equals(2))
                        GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_STRATEGY_PROPOSAL_GLOBAL, Commons.SUBCAT_UPGRADE), Commons.CAT_STRATEGY_PROPOSAL_GLOBAL + "." + Commons.SUBCAT_UPGRADE);
                    else
                        GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_STRATEGY_PROPOSAL_GLOBAL, Commons.SUBCAT_HOUSES), Commons.CAT_STRATEGY_PROPOSAL_GLOBAL + "" + Commons.SUBCAT_HOUSES);
                }
                else if (proposalHouses < numTurnsInLevel / 3) //if a complete round did not have any residential constructions
                {
                    GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_STRATEGY_PROPOSAL_GLOBAL, Commons.SUBCAT_HOUSES), Commons.CAT_STRATEGY_PROPOSAL_GLOBAL + "." + Commons.SUBCAT_HOUSES);
                }
                else if (proposalPolicies < 1)
                {
                    GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_STRATEGY_PROPOSAL_GLOBAL, Commons.SUBCAT_POLICY), Commons.CAT_STRATEGY_PROPOSAL_GLOBAL + "." + Commons.SUBCAT_POLICY);
                }
                else
                    GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_STRATEGY_PROPOSAL_GLOBAL, Commons.SUBCAT_UPGRADE), Commons.CAT_STRATEGY_PROPOSAL_GLOBAL + "." + Commons.SUBCAT_UPGRADE);
            }
            else
                playCollaborativeEvaluation();
        
        }

        void playCollaborativeEvaluation()
        {   //compares if there is one individual score that is two times lower than the others
            if((gameState.EnercitiesInfo.EconomyScore * 2 < gameState.EnercitiesInfo.EnvironmentScore &&
                gameState.EnercitiesInfo.EconomyScore * 2 < gameState.EnercitiesInfo.WellbeingScore) ||
                (gameState.EnercitiesInfo.EnvironmentScore * 2 < gameState.EnercitiesInfo.EconomyScore &&
                gameState.EnercitiesInfo.EnvironmentScore * 2 < gameState.EnercitiesInfo.WellbeingScore) ||
                (gameState.EnercitiesInfo.WellbeingScore * 2 < gameState.EnercitiesInfo.EconomyScore &&
                gameState.EnercitiesInfo.WellbeingScore * 2 < gameState.EnercitiesInfo.EnvironmentScore))
                GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_COLLABORATIVE_EVALUATION, Commons.SUBCAT_LOW_SCORES), Commons.CAT_COLLABORATIVE_EVALUATION + "." + Commons.SUBCAT_LOW_SCORES);
            else
                GamePublisher.PerformUtterance("", GetUtterance(Commons.CAT_COLLABORATIVE_EVALUATION, Commons.SUBCAT_GOOD_SCORES), Commons.CAT_COLLABORATIVE_EVALUATION + "." + Commons.SUBCAT_GOOD_SCORES);

        }

        void SetPlayersGender(Gender player1gender, Gender player2gender)
        {
            this.Player1Gender = player1gender;
            this.Player2Gender = player2gender;
            gameState.Player1.Gender = this.Player1Gender;
            gameState.Player2.Gender = this.Player2Gender;
        }

        void IEnercitiesGameStateEvents.PlayersGender(Gender player1gender, Gender player2gender)
        {
            SetPlayersGender(player1gender, player2gender);   
        }

        #region ITargetEvents Members

        void ITargetEvents.TargetLink(string targetName, string linkedTargetName)
        {
        }

        void ITargetEvents.TargetScreenInfo(string targetName, int X, int Y)
        {
            if (!SavedTargets.ContainsKey(targetName)) SavedTargets.Add(targetName, new XYInt(X, Y));
            else SavedTargets[targetName] = new XYInt(X, Y);
        }

        #endregion



        void IEnercitiesGameStateEvents.PlayersGenderString(string player1gender, string player2gender)
        {
            Gender gender1 = Gender.Female;
            Gender gender2 = Gender.Female;
            if (player1gender.ToLower() == "male" || player1gender.ToLower() == "m") gender1 = Gender.Male;
            if (player2gender.ToLower() == "male" || player1gender.ToLower() == "m") gender2 = Gender.Male;
            this.SetPlayersGender(gender1, gender2);
        }


        #region IEnercitiesTaskEvents Members


        void IEnercitiesTaskEvents.ExamineCell(double screenX, double screenY, int cellX, int cellY, StructureType StructureType_structure, string StructureType_translated)
        {
        }

        #endregion
        

#region not used
        
        public void BestActionsPlanned(EnercitiesRole currentPlayer, string[] EnercitiesActionInfo_actionInfos)
        {
            //throw new NotImplementedException();
        }

        void Thalamus.BML.ISpeakEvents.SpeakStarted(string id)
        {
            //throw new NotImplementedException();
        }


        void IEnercitiesAIActions.PredictedValuesUpdated(double[] values)
        {
            //throw new NotImplementedException();
        }
#endregion

        void IEnercitiesAIActions.StrategiesUpdated(string StrategiesSet_strategies)
        {
            throw new NotImplementedException();
        }

        void IEnercitiesAIActions.BestActionPlanned(string[] EnercitiesActionInfo_actionInfos)
        {
            throw new NotImplementedException();
        }

        void IEnercitiesAIActions.BestActionsPlanned(EnercitiesRole currentPlayer, string[] EnercitiesActionInfo_actionInfos)
        {
            throw new NotImplementedException();
        }

        void IEnercitiesAIActions.ActionsPlanned(EnercitiesRole currentPlayer, string Strategy_planStrategy, string[] EnercitiesActionInfo_bestActionInfos, string[] EnercitiesActionInfo_worstActionInfos)
        {
            throw new NotImplementedException();
        }
    }

}
