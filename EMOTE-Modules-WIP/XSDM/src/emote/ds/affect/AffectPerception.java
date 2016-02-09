package emote.ds.affect;

import org.json.JSONException;
import org.json.JSONObject;

public class AffectPerception {
	
	public JSONObject run(){
		JSONObject out = new JSONObject();
		Double r = Math.random();
		
		
		if (r >= 0.0 && r < 0.1){
			try {
				//frustrated
				out.put("fromModule", "affect");
				out.put("valence", "negative");
				out.put("arousal", "positive");
				out.put("confidence", 800.0);
			} catch (JSONException e) {
				e.printStackTrace();
			}
		} 
		else if (r >= 0.1 && r < 0.2){
			try {
				//bored
				out.put("fromModule", "affect");
				out.put("valence", "negative");
				out.put("arousal", "negative");
				out.put("confidence", 800.0);
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		else if (r >= 0.2 && r < 0.3){
			try {
				//happy
				out.put("fromModule", "affect");
				out.put("valence", "positive");
				out.put("arousal", "positive");
				out.put("confidence", 800.0);
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		else if (r >= 0.3 && r < 0.4){
			try {
				//calm
				out.put("fromModule", "affect");
				out.put("valence", "positive");
				out.put("arousal", "negative");
				out.put("confidence", 800.0);
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		else {
			try {
				//calm
				out.put("fromModule", "affect");
				out.put("valence", "neutral");
				out.put("arousal", "neutral");
				out.put("confidence", 800.0);
			} catch (JSONException e) {
				e.printStackTrace();
			}
		} 
		
		/*
		try {
			//calm
			out.put("fromModule", "affect");
			out.put("valence", "neutral");
			out.put("arousal", "neutral");
			out.put("confidence", 800.0);
		} catch (JSONException e) {
			e.printStackTrace();
		}*/
		
		return out;
	}
}

