package emote.ds.test;

import java.io.File;
import java.io.IOException;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import emote.ds.affect.AffectPerception;
import emote.ds.im.DialogueManager;
import emote.ds.im.Metric;
import emote.scenario2.Scenario2GameAI;
import emote.scenario2.Scenario2Enercities;
import emote.scenario2.Skene;

public class TestEnvScenario2 {
	DialogueManager system;
	AffectPerception affect;
	Skene skene;
	Scenario2GameAI gameAI;
	Scenario2Enercities enercities;
	
	public TestEnvScenario2(){
		
	}
	
	
	public static void main(String[] args){
		TestEnvScenario2 u = new TestEnvScenario2();
		try {
			u.run();
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}
	
	
	public void run() throws JSONException{
		
		Boolean step_by_step_exec = true;
		
		
		JSONObject systemInput;
		JSONArray systemOutput;
		systemInput = new JSONObject();
		
		Integer totalEpisodes = 1;
		//Integer reduceIn = 500; //reduce epsilon by half in 500 episodes
		Integer currentEpisode = 1;
	
		system = new DialogueManager(new File("."), "scripts/scenario2emote.xml");
		
		gameAI = new Scenario2GameAI();
		skene = new Skene();
		affect = new AffectPerception();
		boolean sendtimeout = false;
		
		//double epsilon = 0.8;
		//system.setEpsilon(epsilon);
		
		do {
			system.reset();
			skene.reset();
			
			do {
				
				System.out.println("");
				System.out.println("");
				
				systemOutput = system.run(systemInput);
				system.updateQValues(0.0);
				System.out.println("SYSTEM: " + systemOutput);
				
				
				
				
				
				for (int i=0; i<systemOutput.length(); i++){
					
					
					//system.displayDialogueState();
					JSONObject systemOutputDA = systemOutput.getJSONObject(i);
					if (systemOutputDA.has("utterance")) { 
						System.out.println("SYSTEM: " + systemOutputDA.getString("utterance"));
					}

					if (systemOutputDA.has("toModule")){
						try {
							if (systemOutputDA.getString("toModule").equals("gameAI")){
								JSONObject gameAIOutput = gameAI.run(systemOutputDA);
								System.out.println("GameAI:" + gameAIOutput);
								systemInput = gameAIOutput;
							}
							else if (systemOutputDA.getString("toModule").equals("skene")){
								systemInput = skene.run(systemOutputDA);
								System.out.println("Skene out:" + systemInput);
								system.displayDialogueState();
								systemOutput = system.run(systemInput);
								System.out.println("SYSTEM2:" + systemOutput.toString());
								systemInput = new JSONObject();
								systemInput.put("fromModule", "tts");
								systemInput.put("utteranceStatus", "delivered");
							} else {
								systemOutput = null;
							}
							
						} catch (JSONException e) {
							e.printStackTrace();
						}
					}
				}	
				/*following code allows step-by-step execution and monitoring*/
				if (step_by_step_exec){
					try {
						System.in.read();
					} catch (IOException e) {
						e.printStackTrace();
					}
				}
				
			} while (systemOutput != null);
			
			
			Metric m = system.getMetrics();
			Double reward = 1000.0;
			try {
				Double t = (Double) m.getValue("turns");
				Double nf = 0.0;
				Double gc = 0.0;
				reward = reward - (nf*10) + (gc*5);
				system.updateQValues(reward);
			} catch (JSONException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
			currentEpisode++;
			
			/* System.out.println(currentEpisode + "," + reward);
			if (reduceIn != 0 && currentEpisode % reduceIn == 0){
				epsilon = epsilon/2.0;
				reduceIn = reduceIn/2;
				system.setEpsilon(epsilon);
				System.out.println("Epsilon set to:" + epsilon);
			} */
		} while (currentEpisode < totalEpisodes);
		
		system.savePolicy();
	}
	
	
}
