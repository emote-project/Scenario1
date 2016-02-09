package emote.scenario2;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;
import org.json.JSONException;
import org.json.JSONObject;

import emote.ds.im.DialogueManager;

public class Scenario2Enercities {
	
	Logger logger = Logger.getLogger(Scenario2Enercities.class.getName());
	
	public Scenario2Enercities(){
		PropertyConfigurator.configure("log4j.properties");
	}
	
	public JSONObject run(JSONObject in){
		String requestType = "null";
		String action = "null";

		JSONObject out = new JSONObject();
		
		try {
			out.put("fromModule", "enercities");
			if (in.has("communicativeFunction")){
				requestType = in.getString("communicativeFunction");
			}
			if (in.has("gameAction")){
				action = in.getString("gameAction");
			}
			
			if (requestType.equals("executeGameAction")){ 
				logger.info("Enercities: Game action executed:" + action);
				out.put("gameActionExecuted", "true");
				out.put("gameAction", "confirmConstruction");
				out.put("gameStructure", in.getString("gameStructure"));
			}
			
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return out;
	}
}
