package emote.ds.im;
import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Hashtable;
import java.util.Iterator;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;




public class DialoguePolicy{
	
	JSONArray policy;
	DialogueState currentDS, lastDS;
	String currentActionId, lastActionId;
	Double epsilon;

	Logger logger = Logger.getLogger(DialoguePolicy.class.getName());
	
	
	
	public DialoguePolicy(DialogueState d, Rules r){
		PropertyConfigurator.configure("log4j.properties");
		policy = policyMaker(d,r);
		lastDS = null;
		lastActionId = null;
		currentDS = null;
		currentActionId = null;
		epsilon = 0.8;
	}
	
	/*
	 * Get policy entry that will match the given dialogue state
	 */
	private JSONObject getPolicyEntry(DialogueState ds) throws JSONException{
		for (int i=0; i< policy.length();i++){
			JSONObject entry = policy.getJSONObject(i);
			JSONArray state = entry.getJSONArray("state");
			Boolean match = true;
			//match the states in the policy to see which one matches to current dialogue state
			for (int j=0; j<state.length();j++){
				JSONObject temp = state.getJSONObject(j);
				String name = temp.getString("name");
				if (temp.getString("value").equals("*")){
					continue;
				}
				if (!temp.getString("value").equals((String) ds.getValue(name))){
					match = false;
					break;
				}
			}
			if (match){
				return entry;
			}
		}
		return null;
	}

	public String selectActionToExecute(DialogueState ds, Boolean randomSelection) throws JSONException{
		//ds.display();
		
		JSONObject entry = getPolicyEntry(ds);
		if (entry != null){
			//we found the entry in the policy that matches the current dialogue state.. lets find an action to execute
			//logger.info("Policy state:" + entry.getJSONArray("state"));
			//logger.info("Possible actions:" + entry.getJSONArray("possibleActions"));
			JSONArray possibleActions = entry.getJSONArray("possibleActions");
			JSONObject action = null;
			String actionId = null;
			if (possibleActions.length() == 1){
				action = possibleActions.getJSONObject(0);
			} 
			else if (possibleActions.length() > 1){
				int index;
				if (randomSelection){
					index = (int) Math.floor(Math.random() * possibleActions.length());
				} else {
					//if randomSelection is false, do epsilon selection
					if (Math.random() < epsilon){
						//select randomly to allow for exploration
						index = (int) Math.floor(Math.random() * possibleActions.length());
					} else {
						//getting action with highest q-value
						index = 0;
						Double maxQ = 0.0;
						for (int i2 = 0; i2 < possibleActions.length(); i2++){
							JSONObject action1 = possibleActions.getJSONObject(i2);
							double q = action1.getDouble("qValue");
							if (q > maxQ){
								maxQ = q;
								index = i2;
							}
						}
					}
				}
				//logger.info("Action index:" + index);
				action = possibleActions.getJSONObject(index);
			}
			if (action != null){
				actionId = action.getString("actionId");
			}
			
			lastDS = currentDS;
			lastActionId = currentActionId;
			
			currentDS = ds;
			currentActionId = actionId;
			
			return actionId;
		}
		return null;
	}
	
	public void storePolicy(){
		try{
			FileWriter fstream = new FileWriter("policy.txt");
			BufferedWriter out = new BufferedWriter(fstream);
			for (int i=0; i<policy.length();i++){
				out.write(policy.get(i).toString() + "\n");
			}  
			out.close();
		} catch (Exception e){
			e.printStackTrace();
		}
	}

	private double getQValue(DialogueState ds, String actionId) throws JSONException{
		JSONObject entry = getPolicyEntry(ds);
		if (entry != null){
			//we found the entry in the policy that matches the current dialogue state.. lets find an action to execute
			//logger.info("Policy state:" + entry.getJSONArray("state"));
			//logger.info("Possible actions:" + entry.getJSONArray("possibleActions"));
			JSONArray possibleActions = entry.getJSONArray("possibleActions");
			
			
			for (int i2 = 0; i2 < possibleActions.length(); i2++){
				JSONObject action1 = possibleActions.getJSONObject(i2);
				String aId = action1.getString("actionId");
				if (aId.equals(actionId)){
					return action1.getDouble("qValue");
				}
			}
		}
		return 0.0;
	}
	
	private void setQValue(DialogueState ds, String actionId, Double q) throws JSONException{
		JSONObject entry = getPolicyEntry(ds);
		if (entry != null){
			//we found the entry in the policy that matches the current dialogue state.. lets find an action to execute
			//logger.info("Policy state:" + entry.getJSONArray("state"));
			//logger.info("Possible actions:" + entry.getJSONArray("possibleActions"));
			JSONArray possibleActions = entry.getJSONArray("possibleActions");
			
			
			for (int i2 = 0; i2 < possibleActions.length(); i2++){
				JSONObject action1 = possibleActions.getJSONObject(i2);
				String aId = action1.getString("actionId");
				if (aId.equals(actionId)){
					action1.put("qValue", q);
				}
			}
			//logger.info("Updated q:" + entry.toString());
		}
	}
	
	public void updateQValues(Double r) throws JSONException {
		if (lastDS != null && currentDS != null){
			Double alpha = 0.3;
			Double oldQ = getQValue(lastDS,lastActionId);
			Double newQ = oldQ + alpha * (r + getQValue(currentDS,currentActionId) - oldQ);
			
			setQValue(currentDS, currentActionId, newQ);
		}
	}
	
	
	

	public JSONArray policyMaker(DialogueState d, Rules r){
		
		JSONArray p = new JSONArray();
		try {
			
			//enumerate dialogue states
			for (int i = 0; i < d.length(); i++){
				
				JSONObject j = d.getVariable(i);
				
				//get the range for each variable
				ArrayList<String> range = null;
				if (j.has("range")){
					String ra = j.getString("range");
					range = new ArrayList<String>(Arrays.asList(ra.split((","))));
				} 
				else if (j.has("type") && j.getString("type").equals("boolean")){
					range = new ArrayList<String>();
					range.add("true");
					range.add("false");
				}
				else {
					range = new ArrayList<String>();
					range.add("*");
					range.add("null");
				}
				
				
				JSONArray temp = new JSONArray();
				//temp array will hold new states where we extend the variables by one more..
				if (p.length() == 0){ //if p is empty.. which originally it is..
					JSONObject m = null;
					for (int k = 0; k < range.size(); k++){
						m = new JSONObject();
						m.put(j.getString("name"), range.get(k));
						temp.put(m);
					}
				} else {
					for (int k2 = 0; k2 < p.length(); k2++){
						for (int k = 0; k < range.size(); k++){
							//need to copy the JSONObjects explicitly as below
							//because just assigning n to m does not work.. :(
							JSONObject n = p.getJSONObject(k2);
							JSONObject m = new JSONObject();
							Iterator<String> keys = n.keys();
							while(keys.hasNext()){
								String key1 = keys.next();
								m.put(key1, n.get(key1));
							}
							m.put(j.getString("name"), range.get(k));
							temp.put(m);
						}
					}
				}
				p = temp;
			}
			logger.info("States enumerated!");
			policy = new JSONArray();
			
			for (int i = 0; i < p.length(); i++){
				JSONObject policyEntry = new JSONObject();
				
				//converting jsonobject into jsonarray in the same format as dialogue state
				JSONArray ds = new JSONArray();
				JSONObject dstemp = p.getJSONObject(i);
				Iterator<String> keys = dstemp.keys();
				while (keys.hasNext()){
					String ke = keys.next();
					JSONObject temp = new JSONObject();
					temp.put("name", ke);
					temp.put("value", dstemp.get(ke));
					ds.put(temp);
				}
				policyEntry.put("state", ds);
				
				JSONArray possibleActions = new JSONArray();
				
				if (r.length() > 0){
					for (int ri=0; ri<r.length(); ri++){
						Rule rule = r.getRule(ri);
						DialogueState dstate = new DialogueState(ds);
						Domains dom = new Domains();
						dom.d = dstate;
						Boolean ruleSatisfied = rule.checkPrecondition(dom);
						if (ruleSatisfied){
							JSONObject action = new JSONObject();
							action.put("actionId", rule.id);
							action.put("qValue", Math.random());
							possibleActions.put(action);
						}
					}
				}
				policyEntry.put("possibleActions", possibleActions);
				policy.put(policyEntry);
			}
			
			logger.info("Policy created for dialogue states: " + p.length());
			
			
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return policy;
	}

	public void setEpsilon(Double e) {
		epsilon = e;
	}
}