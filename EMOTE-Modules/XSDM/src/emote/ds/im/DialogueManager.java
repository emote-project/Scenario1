
package emote.ds.im;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Date;
import java.util.Iterator;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import emote.ds.script.ScriptReader;



/**
 * @author srinijanarthanam
 *
 */
public class DialogueManager {
	
	Domains d;
	Rules uRules;
	Strategies actionStrategies;
	DialoguePolicy policy;
	JSONObject properties;
	ScriptReader sr;
	String scriptFile;
	File scriptsDir;
	int run;
	Boolean running;
	
	Logger logger = Logger.getLogger(DialogueManager.class.getName());
	
	public DialogueManager(File dir, String filename){
		
		PropertyConfigurator.configure("log4j.properties");
		Date now = new Date();
		
		logger.info("NEW DM CREATED: " + (now).toString());
		
		sr = new ScriptReader(dir, filename);
		scriptFile = filename;
		scriptsDir = dir;
		
		d = new Domains();
		uRules = new Rules();
		actionStrategies = new Strategies();
		
		uRules.setRules(sr.getUpdateRules());
		/* 
		 
		try {
			uRules.display();
		} catch (JSONException e1) {
			e1.printStackTrace();
		}
		*/
		
		actionStrategies.setActionStrategies(sr.getActionStrategies());
		/*
		try {
			aRules.display();
		} catch (JSONException e1) {
			e1.printStackTrace();
		}*/
		
		properties = sr.getProperties();
		
		//logger.info("all loaded");
		DialogueState ds = new DialogueState();
		ds.setStateVariables(sr.getDialogueState());
		policy = null;
		/* 
		try {
			if (properties.getBoolean("dialoguePolicy")){
				policy = new DialoguePolicy(ds,aRules);
				policy.storePolicy();
			} else {
				policy = null;
			}
		
		} catch (JSONException e) {
			e.printStackTrace();
		}*/
		
		run = 0;
		running = false;
	}
	
	
	public void close(){
		running = false;
		Date now = new Date();
		logger.info("DM STOPPED: " + (now).toString());
		logger.info("");
		logger.info("");
		logger.info("");
	}
	
	public void setDStateVariable(String var, Object val){
		JSONObject t2 = new JSONObject();
		try {
			t2.put("name", var);
			t2.put("value", val);
			d.d.setValue(t2);
			//d.d.display();
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}
	
	public void reset(){
		sr = null;
		sr = new ScriptReader(scriptsDir, scriptFile);
		
		d.d.setStateVariables(sr.getDialogueState());
		d.iv.setInputVariables(sr.getInputVariables());
		d.iv.setDefaultInput(sr.getInputVariables());
		d.ov.setOutputVariables(sr.getOutputVariables());
		d.mt.setStateVariables(sr.getMetrics());
		logger.info("Reset");
	}
	
	
	public JSONArray run(JSONObject in){
		running = true;
		logger.info("DM run:" + run + ":" + System.currentTimeMillis());
		
		logger.info("DM input :" + in.toString());
		JSONArray out = null;
		
		try {
			updateInputVariables(in);
			executeUpdateRules();
			//d.iv.display();
			//d.d.display();
			//state is updated now..
			d.ov.clear();
			
			
			out = selectAndExecuteAction();
			displayDialogueState();
			//action selected and executed now.
			
		} catch (JSONException e) {
			e.printStackTrace();
		}
		
		//ov.display();
		logger.info("DM output:" + out.toString());
		run++;
		running = false;
		return out;
	}

	public Boolean isRunning(){
		return running;
	}
	
	public void displayDialogueState(){
		try {
			d.d.display();
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	private JSONArray selectAndExecuteAction() throws JSONException {
		JSONArray out = null;
		if (actionStrategies.isEmpty()){ return out; }
		
		if (policy != null){
			//set second parameter to true if the selection is to be random
			// false if max q
			String id = policy.selectActionToExecute(d.d, true);
			//logger.info("Action selected:" + id);
			//Rule rule = aRules.getRule(id);
			//rule.executeActions(d);
		} else {
			//no policy
			
			//first find the main strategy to start with
			actionStrategies.executeStrategy("main", d);
			d.ov.deleteUnusedOutputDA();
			
		}
		
		
		out = d.ov.getAllDA();
		return out;
	}

	public void updateInputVariables(JSONObject in) throws JSONException{
		d.iv.refresh();
		//d.iv.display();
		if (in != null && in.length() > 0){
			
			@SuppressWarnings("unchecked")
			Iterator<String> it = in.keys();
			while (it.hasNext()){
				String tempKey = it.next();
				JSONObject temp = new JSONObject();
				temp.put("name", tempKey);
				temp.put("value", in.get(tempKey));
				d.iv.setValue(temp);
			}
		} else {
			//logger.info("NO input!");
			JSONObject temp = new JSONObject();
			temp.put("name", "fromModule");
			temp.put("value", "null");
			d.iv.setValue(temp);
		}
		//d.iv.display();
	}
	
	public void executeUpdateRules() throws JSONException {
		
		if (uRules.isEmpty()){ return; }
		
		for (int i = 0; i < uRules.length(); i++){
			Rule rule = uRules.getRule(i);
			//logger.info("Checking update rule: " + rule.comment);
			Boolean preconditionTrue = rule.checkPrecondition(d);
			
			if (preconditionTrue){
				//logger.info("Executing update rule: " + rule.comment);
				ActionAnd aa = rule.selectAction();
				
				//enumerate all the actions in aa
				for (int j = 0; j < aa.length(); j++){
					Action action = aa.getAction(j);
					
					//enumerate all actionunits (e.g. assignment) in
					for (int k = 0; k < action.length(); k++){
						JSONObject assignee, assigner;
						assignee = null; assigner = null;
						JSONObject temp = action.getJSONObject(k);
						if (temp.has("type") && temp.getString("type").equals("assignment")){
							if (temp.has("assigner")){
								assigner = temp.getJSONObject("assigner");
								assignee = temp.getJSONObject("assignee");
								//make the assignment
								JSONObject t2 = new JSONObject();
								t2.put("name", assignee.getString("var"));
								t2.put("value", this.getAssignmentValue(assigner, d));
								
								//which domain should be updated..??
								if (assignee.has("domain") && assignee.getString("domain").equals("dstate") && t2.has("value")){
									d.d.setValue(t2);
								}
								if (assignee.has("domain") && assignee.getString("domain").equals("output") && t2.has("value")){
									d.ov.setValue(t2);
								}
								if (assignee.has("domain") && assignee.getString("domain").equals("metric") && t2.has("value")){
									d.mt.setValue(t2);
								}
							}
						} 
					} //end of for k
				}//end of for j
				logger.info("Executed update rule:" + rule.comment);
			}
		}
		//d.d.display();
		//d.mt.display();
	}
	
	public Metric getMetrics(){
		return d.mt;
	}


	public void updateQValues(Double r) {
		if (policy != null){
			try {
				policy.updateQValues(r);
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
	}
	
	public void setEpsilon(Double e){
		if (policy != null){
			policy.setEpsilon(e);
		}
	}

	public void savePolicy() {
		if (policy != null){
			policy.storePolicy();
		}
	}
	
	private Object getAssignmentValue(JSONObject assigner, Domains d) throws JSONException{
		//is the assigner a variable?
		if (assigner.has("var")){
			//is the assigner variable in the input domain??
			if (assigner.has("domain") && assigner.getString("domain").equals("input")){
				return d.iv.getValue(assigner.getString("var"));
			}
			
			//is the assigner variable in the dstate domain??
			if (assigner.has("domain") && assigner.getString("domain").equals("dstate")){
				return d.d.getValue(assigner.getString("var"));
			}
			
			//is the assigner variable in the metric domain??
			if (assigner.has("domain") && assigner.getString("domain").equals("metric")){
				return d.mt.getValue(assigner.getString("var"));
			}
		}
		//is the assigner a value
		else if (assigner.has("value")){
			return assigner.get("value");
		}
		//is the assigner a function
		else if (assigner.has("function")){
			if (assigner.getString("function").equals("randomBoolean")){
				return ((Math.random() > 0.5) ? "true":"false"); 
			}
			if (assigner.getString("function").equals("randomChoice")){
				String[] range = assigner.getString("range").split(",");
				return (range[(int) (Math.random() * range.length)]); 
			}
		}
		//math
		else if (assigner.has("add") || assigner.has("subtract")){
			if (assigner.has("add")){
				JSONArray arg = assigner.getJSONArray("add");
				Double sum = 0.0;
				for (int i = 0; i< arg.length(); i++){
					JSONObject jb = arg.getJSONObject(i);
					Object ob = getAssignmentValue(jb,d);
					Double o = (Double) ob;
					sum += o;
				}
				return sum;
			}
			if (assigner.has("subtract")){
				JSONArray arg = assigner.getJSONArray("subtract");
				JSONObject jb = arg.getJSONObject(0);
				Double sum = Double.valueOf((String) getAssignmentValue(jb,d));
				for (int i = 0; i< arg.length(); i++){
					jb = arg.getJSONObject(i);
					Double o = Double.valueOf((String) getAssignmentValue(jb,d));
					sum -= o;
				}
				return sum;
			}
		}
		else if (assigner.has("array")){
			JSONObject array = assigner.getJSONObject("array");
			//logger.info("Got the array def:" + array.toString());
			Integer in = null;
			if (array.has("index")){
				in = (int) Math.round((Double) getAssignmentValue(array.getJSONObject("index"),d));
				//logger.info("Got the index:" + in);
			}
			JSONArray a = null;
			//is the assigner variable in the dstate domain??
			if (array.has("domain") && array.getString("domain").equals("dstate")){
				a = (JSONArray) d.d.getValue(array.getString("var"));
				//logger.info("Got the array:" + a.toString());
			}
			return a.get(in);
		}
		return null;
	}
}
	
	