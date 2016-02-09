package emote.ds.im;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class Strategies {
	JSONArray strategies;

	Logger logger = Logger.getLogger(Strategies.class.getName());
	
	
	public Strategies(){
		PropertyConfigurator.configure("log4j.properties");
	}

	public JSONArray getActionStrategies() {
		return this.strategies;
		
	}

	public void setActionStrategies(JSONArray actionStrategies) {
		strategies = actionStrategies;
		
	}

	public boolean isEmpty(){
		if (strategies == null || strategies.length() == 0){
			return true;
		}
		return false;
	}

	public int length() {
		return strategies.length();
	}

	public Strategy getStrategy(int i) {
		if (strategies.length() > i){
			try {
				return (Strategy) strategies.get(i);
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		return null;
	}
	
	public Strategy getStrategy(String id) {
		for (int i=0; i<strategies.length();i++){
			Strategy r;
			try {
				r = (Strategy) strategies.get(i);
				if (r.id.equals(id)){
					return r;
				}
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		return null;
	}
	
	public void executeStrategy(String strategyName, Domains d) throws JSONException {
		Strategy str = getStrategy(strategyName);
		
		Rules aRules = str.getRules();
		
		//apply the first rule that satisfies the precondition
		for (int i = 0; i < aRules.length(); i++){
			//first find the main strategy to start with
			Rule rule = aRules.getRule(i);
			//d.d.display();
			//logger.info("Checking action rule:" + rule.comment);
			if (rule.checkPrecondition(d)){
				String id = "null";
				logger.info("Executing action rule:" + strategyName + ":" + rule.comment);
				ActionAnd aa = rule.selectAction();
				
				//enumerate all the actions in aa
				for (int j = 0; j < aa.length(); j++){
					Action action = aa.getAction(j);
					
					
					double r = Math.random();
					if (r < action.getQProbability()){
						d.ov.createNewOutputDA();
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
							else if (temp.has("type") && temp.getString("type").equals("gotoStrategy")){
								String strId = temp.getString("id");
								executeStrategy(strId, d);
							}
							
						} //end of for k
						if (d.ov.length() > 0){
							//copy all the dialogueState var (where copyToOutput=true) to output
							for (int m=0; m < d.d.svar.length(); m++){
								JSONObject temp = d.d.svar.getJSONObject(m);
								if (temp.has("copyToOutput") && temp.getString("copyToOutput").equals("true")){
									//logger.info("Found comething to copy to output:" + temp.getString("name"));
									JSONObject t2 = new JSONObject();
									t2.put("name", temp.getString("name"));
									if (temp.getString("type").equals("number")){
										t2.put("value", temp.get("value"));
									} else {
										t2.put("value", temp.getString("value"));
									}
									d.ov.setValue(t2);
								}
							}
						}
					}
					
					
				}//end of for j
				
				
				
				break;
			}
		} // all rules
		
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
