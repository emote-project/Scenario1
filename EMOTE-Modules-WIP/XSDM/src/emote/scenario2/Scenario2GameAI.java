package emote.scenario2;

import org.json.JSONException;
import org.json.JSONObject;

public class Scenario2GameAI {
	public JSONObject run(JSONObject in){
		String requestType = "null";
		String player = "null";
		String strategy = "null"; 
		
		JSONObject out = new JSONObject();
		try {
			if (in.has("communicativeFunction")){
				requestType = in.getString("communicativeFunction");
			}
			if (in.has("fromModule")){
				player = in.getString("fromModule");
			}
			if (in.has("strategy")){
				strategy = in.getString("strategy");
			}
			out.put("fromModule", "gameAI");
				
			if (requestType.equals("getNextMove")){ 
				if (strategy.equals("Global")){
					Double r = Math.random();
					if (r < 0.25){
						out.put("nextBestGameAction", "confirmConstruction");
						out.put("gameStructure", "Market");
						out.put("gamePositionX", "1");
						out.put("gamePositionY", "1");
					} 
					else if (r < 0.5){
						out.put("nextBestGameAction", "confirmConstruction");
						out.put("gameStructure", "Windmills");
						out.put("gamePositionX", "1");
						out.put("gamePositionY", "1");
					}
					else if (r < 0.75){
						out.put("nextBestGameAction", "confirmConstruction");
						out.put("gameStructure", "Suburban");
						out.put("gamePositionX", "1");
						out.put("gamePositionY", "1");
					}
					else if (r <= 1.0){
						out.put("nextBestGameAction", "confirmConstruction");
						out.put("gameStructure", "Coal_Plant_Small");
						out.put("gamePositionX", "1");
						out.put("gamePositionY", "1");
					}
					
				}
				else if (strategy.equals("Environmentalist")){
					Double r = Math.random();
					if (r < 0.25){
						out.put("nextBestGameAction", "confirmConstruction");
						out.put("gameStructure", "Park");
						out.put("gamePositionX", "1");
						out.put("gamePositionY", "1");
					} 
					else if (r < 0.5){
						out.put("nextBestGameAction", "confirmConstruction");
						out.put("gameStructure", "Windmills");
						out.put("gamePositionX", "1");
						out.put("gamePositionY", "1");
					}
					else if (r < 0.75){
						out.put("nextBestGameAction", "confirmConstruction");
						out.put("gameStructure", "Suburban");
						out.put("gamePositionX", "1");
						out.put("gamePositionY", "1");
					}
					else if (r <= 1.0){
						out.put("nextBestGameAction", "confirmConstruction");
						out.put("gameStructure", "Coal_Plant_Small");
						out.put("gamePositionX", "1");
						out.put("gamePositionY", "1");
					}
				} 
				else {
					Double r = Math.random();
					if (r < 0.25){
						out.put("nextBestGameAction", "confirmConstruction");
						out.put("gameStructure", "Light_Industry");
						out.put("gamePositionX", "1");
						out.put("gamePositionY", "1");
					} 
					else if (r < 0.5){
						out.put("nextBestGameAction", "confirmConstruction");
						out.put("gameStructure", "Windmills");
						out.put("gamePositionX", "1");
						out.put("gamePositionY", "1");
					}
					else if (r < 0.75){
						out.put("nextBestGameAction", "confirmConstruction");
						out.put("gameStructure", "Suburban");
						out.put("gamePositionX", "1");
						out.put("gamePositionY", "1");
					}
					else if (r <= 1.0){
						out.put("nextBestGameAction", "confirmConstruction");
						out.put("gameStructure", "Coal_Plant_Small");
						out.put("gamePositionX", "1");
						out.put("gamePositionY", "1");
					}
				}
				
			}
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return out;
	}
}
