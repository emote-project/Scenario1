package uk.ac.hw.emote.intman.listener;

import org.eclipse.jetty.util.log.Log;
import org.json.JSONException;
import org.json.JSONObject;

import redstone.xmlrpc.XmlRpcArray;
import uk.ac.hw.emote.intman.dm.TurnTakingManager;
import emoteenercitiesmessages.Gender;
import emoteenercitiesmessages.IEnercitiesAIActions;
import emoteenercitiesmessages.IEnercitiesGameStateEvents;
import emoteenercitiesmessages.IEnercitiesTaskEvents;

public class EnercitiesListener implements IEnercitiesAIActions, IEnercitiesGameStateEvents,
        IEnercitiesTaskEvents {

    @Override
    public void ConfirmConstruction (String structure, String translation, int cellX, int cellY) {
        Log.getLog ().info ("Confirm construction");
        /*
        try {
            JSONObject enercitiesInput = new JSONObject (
                    "{gameActionExecuted:\"true\",userGameAction:\"null\",fromModule:enercities}");
            TurnTakingManager.getInstance ().processInputAndPerform (enercitiesInput);
        } catch (JSONException e) {
            e.printStackTrace ();
        }
        */
    }

    @Override
    public void ImplementPolicy (String policy, String translation) {
        // TODO Auto-generated method stub

    }

    @Override
    public void PerformUpgrade (String upgrade, String translation, int cellX, int cellY) {
        // TODO Auto-generated method stub

    }

    @Override
    public void SkipTurn () {
        // TODO Auto-generated method stub

    }

    @Override
    public void ExamineCell (double screenX, double screenY, int cellX, int cellY,
            String StructureType_structure, String StructureType_translated) {
        // TODO Auto-generated method stub

    }

    @Override
    public void PlayersGender (Gender player1Gender, Gender player2Gender) {
        // TODO Auto-generated method stub

    }

    @Override
    public void PlayersGenderString (String player1Gender, String player2Gender) {
        // TODO Auto-generated method stub

    }

    @Override
    public void GameStarted (String player1Name, String player1Role, String player2Name,
            String player2Role) {
        // TODO Auto-generated method stub

    }

    @Override
    public void ResumeGame (String player1Name, String player1Role, String player2Name,
            String player2Role, String serializedGameState) {
        // TODO Auto-generated method stub

    }

    @Override
    public void EndGameSuccessfull (int totalScore) {
        // TODO Auto-generated method stub

    }

    @Override
    public void EndGameNoOil (int totalScore) {
        // TODO Auto-generated method stub

    }

    @Override
    public void EndGameTimeOut (int totalScore) {
        // TODO Auto-generated method stub

    }

    @Override
    public void TurnChanged (String serializedGameState) {
        Log.getLog ().info ("Turn changed: " + serializedGameState);
    }

    @Override
    public void ReachedNewLevel (int level) {
        // TODO Auto-generated method stub

    }

    @Override
    public void StrategyGameMoves (String environmentalistMove, String economistMove,
            String mayorMove, String globalMove) {
        // TODO Auto-generated method stub

    }

    @Override
    public void StrategiesUpdated (String StrategiesSet_strategies) {
        // TODO Auto-generated method stub

    }

    @Override
    public void BestActionPlanned (String[] EnercitiesActionInfo_actionInfos) {
        // TODO Auto-generated method stub

    }

    @Override
    public void BestActionsPlanned (XmlRpcArray EnercitiesActionInfo_actionInfos) {
        Log.getLog ().info ("Best actions: " + EnercitiesActionInfo_actionInfos);
    }

    @Override
    public void PredictedValuesUpdated (double[] values) {
        // TODO Auto-generated method stub

    }

}
