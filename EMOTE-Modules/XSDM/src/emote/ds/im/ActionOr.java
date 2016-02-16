package emote.ds.im;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;
import org.json.JSONArray;

public class ActionOr {
	JSONArray actionAnds;

	Logger logger = Logger.getLogger(ActionOr.class.getName());
	
	
	public ActionOr(){
		PropertyConfigurator.configure("log4j.properties");
		actionAnds = new JSONArray();
	}
	
	public void addActionAnd(ActionAnd aa) {
		actionAnds.put(aa);
	}
	
	public String toString(){
		return actionAnds.toString();
	}
	
	public Integer length(){
		return actionAnds.length();
	}
	
	public JSONArray getActionAnds(){
		return actionAnds;
	}
}
