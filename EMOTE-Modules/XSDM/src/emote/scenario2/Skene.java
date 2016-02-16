package emote.scenario2;

import java.io.File;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import emote.ds.im.DialogueManager;

public class Skene {
	Scenario2Enercities enercities;
	DialogueManager env, eco;
	Scenario2GameAI gameAI;
	
	Logger logger = Logger.getLogger(Skene.class.getName());
	
	public Skene(){
		PropertyConfigurator.configure("log4j.properties");
		
		
		//create two players and simulate them
		gameAI = new Scenario2GameAI();
		enercities = new Scenario2Enercities();
		env = new DialogueManager(new File("."), "scripts/scenario2user.xml");
		eco = new DialogueManager(new File("."), "scripts/scenario2user.xml");
		env.reset();
		eco.reset();
		env.setDStateVariable("userRole", "Environmentalist");
		eco.setDStateVariable("userRole", "Economist");
		
	}
	
	public JSONObject run(JSONObject in){
		JSONObject out = null;
		if (in.has("communicativeFunction")){
			String cf = "null";
			try {
				cf = in.getString("communicativeFunction");
			} catch (JSONException e1) {
				e1.printStackTrace();
			}
			String action="null";
			out = new JSONObject();
			try {
				if (cf.equals("informTurnChange")){
					String turnHolder = in.getString("gameTurnHolder");
					logger.info("Skene: Turn holder changed");
					DialogueManager temp;
					if (turnHolder.equals("Environmentalist")){
						temp = env;
						logger.info("Skene: Its your turn, environmentalist.");
					} else {
						temp = eco;
						logger.info("Skene: Its your turn, economist.");
					}
				
					//user.displayDialogueState();
					JSONObject userOutput = (temp.run(new JSONObject())).getJSONObject(0);
					
					//user.displayDialogueState();
					//user will first call gameAI to identify the next best game action
					if (userOutput.getString("toModule").equals("gameAI")){
						logger.info("GameAI called by user: " + userOutput.toString());
						JSONObject gameAIOutput = gameAI.run(userOutput);
						//run it by the user and get an output
						logger.info("Game AI to " + turnHolder + ":" + gameAIOutput.toString());
						userOutput = (env.run(gameAIOutput)).getJSONObject(0);
						logger.info("User after GameAI:" + userOutput.toString());
						
					}
					
					//user will then tell the game manager to execute the game action
					if (userOutput.getString("toModule").equals("enercities")){
						logger.info("Enercities called by user!:" + userOutput.toString());
						//run it by the user and get an output
						if (userOutput.has("communicativeFunction")){
							cf = userOutput.getString("communicativeFunction");
						}
						if (userOutput.has("gameAction")){
							action = userOutput.getString("gameAction");
						}
						
						if (cf.equals("executeGameAction")){ 
							logger.info("Game action executed for " + turnHolder + ":" + action);
						}
					}
					out.put("fromModule", "enercities");
					out.put("gameAction", action);
					out.put("gameStructure", userOutput.getString("gameStructure"));
				}
				else if (in.getString("communicativeFunction").equals("executeGameAction")){
					if (in.getString("gameAction").equals("confirmConstruction")){
						logger.info("Skene: I have built a " + in.getString("gameStructure"));
						out = enercities.run(in);
					}
				}
				else if (in.getString("communicativeFunction").equals("explainGameAction")){
					String turnHolder = in.getString("gameTurnHolder");
					if (turnHolder.equals("system")){
						logger.info("Skene: I created " + in.getString("gameStructure") + " because ...");
					} else {
						logger.info("Skene: "+ in.getString("gameStructure") + " is good for ...");
					}
					out = new JSONObject();
				}
				
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		return out;
	}

	public void reset(){
		
	}
	
}
