package emote.ds.im;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;



/**
 * @author srinijanarthanam
 *
 */
public class Action {
	
	Double prob, qprob;
	JSONArray actionUnits;
	/* actionUnits can be assignment statements and goto statements */
	
	Logger logger = Logger.getLogger(Action.class.getName());
	
	public Action(){
		PropertyConfigurator.configure("log4j.properties");
		
		prob = 1.0;
		qprob = 1.0;
		actionUnits = new JSONArray(); 
	}
	
	public Action(JSONArray a){
		actionUnits = a;
	}
	
	public void setProbability(double d){
		this.prob = d;
	}
	
	public void setQProbability(double d){
		this.qprob = d;
	}

	public void addActionUnit(JSONObject au){
		actionUnits.put(au);
	}
	
	public JSONArray getActionUnits() {
		return actionUnits;
	}

	public int length() {
		return actionUnits.length();
	}

	public JSONObject getJSONObject(int k) throws JSONException {
		return actionUnits.getJSONObject(k);
	}
	
	public String toString(){
		return "p=" + prob + "," + "q=" + qprob + "," + actionUnits.toString();
	}

	public double getProbability() {
		return this.prob;
	}
	
	public double getQProbability() {
		return this.qprob;
	}
	
}