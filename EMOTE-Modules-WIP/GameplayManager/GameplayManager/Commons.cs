using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameplayManager
{

    class Commons
    {
       
        public const string CAT_CONFIRM_CONSTRUCTION = "ConfirmConstruction";
        public const string CAT_END_GAME = "EndGame";
        public const string CAT_GAME_STARTED = "GameStarted";
        public const string CAT_IMPLEMENT_POLICY = "ImplementPolicy";
        public const string CAT_PERFORM_UPGRADE = "PerformUpgrade";
        public const string CAT_REACHED_NEW_LEVEL = "ReachedNewLevel";
        public const string CAT_SKIP_TURN = "SkipTUrn";
        public const string CAT_TURN_CHANGED = "TurnChanged";
        
        public const string CAT_STRATEGY_PROPOSAL = "StrategyProposal";
        public const string CAT_STRATEGY_PROPOSAL_GLOBAL = "StrategyProposalGlobal";
        public const string CAT_COLLABORATIVE_EVALUATION = "CollaborativeEvaluation";
        public const string CAT_STRATEGY_CONFIRMATION = "StrategyConfirmation";
        public const string CAT_STRATEGY_EXPLORING = "StrategyConfirmation";
        
        public const string SUBCAT_OTHER = "Other";
        public const string SUBCAT_SELF = "Self";
        public const string SUBCAT_SUCCESSFULL = "Successfull";
        public const string SUBCAT_TIMEOUT = "Timeout";
        public const string SUBCAT_LEVEL = "Level";
        public const string SUBCAT_NO_OIL = "NoOil";

         
        public const string SUBCAT_ENVIRONMENTALIST = "Environmentalist";
        public const string SUBCAT_ECONOMIST = "Economist";
        public const string SUBCAT_MAYOR = "Mayor";
        public const string SUBCAT_GLOBAL = "Global";
        public const string SUBCAT_ENERGY = "Energy";
        public const string SUBCAT_UPGRADE = "Upgrade";
        public const string SUBCAT_HOUSES = "Houses";
        public const string SUBCAT_POLICY = "Policy";
        public const string SUBCAT_GENERIC = "Generic";
        public const string SUBCAT_LOW_SCORES = "LowScores";
        public const string SUBCAT_GOOD_SCORES = "GoodScores";
        public const string SUBCAT_MARKET= "Market";
        public const string SUBCAT_STADIUM = "Stadium";
        public const string SUBCAT_WELLBEING = "Wellbeing";
        public const string SUBCAT_POLITIC = "Politic";
        public const string SUBCAT_CONSTRUCTION = "Construction";

        public const string TAG_CURRENT_PLAYER_NAME = "/currentPlayerName/";
        public const string TAG_ECO_NAME = "/ecoName/";
        public const string TAG_ENV_NAME = "/envName/";
        public const string TAG_CURRENT_LEVEL = "/currentLevel/";
        public const string TAG_LEVEL = "/level/";
        public const string TAG_TOTAL_SCORE = "/totalScore/";
        public const string TAG_ENVIRONMENT_SCORE = "/environmentScore/";
        public const string TAG_ECONOMY_SCORE = "/economyScore/";
        public const string TAG_PROPOSED_MOVE = "/proposedMove/";
        public const string TAG_MAYOR_SCORE = "/mayorScore/";
        public const string TAG_CURRENT_MONEY = "/currentMoney/";                   
        public const string TAG_CURRENT_OIL = "/currentOil/";
        public const string TAG_CURRENT_ENERGY = "/currentEnergy/";
        public const string TAG_POLICY = "/policy/";
        public const string TAG_STRUCTURE = "/structure/";
        public const string TAG_UPGRADE = "/upgrade/";
        public const string TAG_STRUCTURE_CATEGORY = "/structureCategory/";
        public const string TAG_ARTIGO_DEFINIDO_ECO = "/artigoDefinidoEco/";
        public const string TAG_ARTIGO_DEFINIDO_ENV = "/artigoDefinidoEnv/";

        public const string TAG_ARTIGO_DEFINIDO_CURR_PLAYER = "/artigoDefinidoCurrentPlayer/";
        public const string TAG_ARTIGO_DEFINIDO_NEXT_PLAYER = "/artigoDefinidoNextPlayer/";
        
                
        public const string TAG_CURRENT_PLAYER_SIDE = "/currentPlayerSide/";
        public const string TAG_CURRENT_PLAYER_ROLE = "/currentPlayerRole/";
        public const string TAG_CURRENT_PLAYER_ROLE_SPEECH = "/currentPlayerRoleSpeech/";
        

        // not present among the event parameters received from enercities
        
        
    }
}
