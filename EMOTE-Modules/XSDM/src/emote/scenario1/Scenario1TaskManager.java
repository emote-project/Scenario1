
package emote.scenario1;

import java.io.File;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.Iterator;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;
import org.dom4j.Attribute;
import org.dom4j.Document;
import org.dom4j.DocumentException;
import org.dom4j.Element;
import org.dom4j.io.SAXReader;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import emote.ds.im.DialogueManager;



/**
 * @author srinijanarthanam
 *
 */
public class Scenario1TaskManager {

	public static void main(String[] arg) throws JSONException{
		Scenario1TaskManager r = new Scenario1TaskManager(new File("scenario"), "Visby_medium.xml");
		r.display();
	}
	
	//Logger logger = Logger.getLogger(Scenario1TaskManager.class.getName());
	
	JSONArray taskSteps;
	Integer index;
	
	public Scenario1TaskManager(File scenariosDir, String file){
		//PropertyConfigurator.configure("log4j.properties");
		
		taskSteps = new JSONArray();
		try {
			File f = new File(scenariosDir, file);
			//logger.info("Loading scenario task script from:" + f.toString());
			URL scriptURL = f.toURI().toURL();
			Document scriptDoc = parse(scriptURL);
			getScenario1Task(scriptDoc);
		} catch (MalformedURLException e) {
			e.printStackTrace();
		} catch (DocumentException e) {
			e.printStackTrace();
		} catch (JSONException e) {
			e.printStackTrace();
		} 
		index = 0;
	}
	
	public void display(){
		for (int i = 0; i<taskSteps.length();i++){
			try {
				//logger.info(taskSteps.getJSONObject(i).toString());
				System.out.println(taskSteps.getJSONObject(i).toString());
			} catch (JSONException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
	
	public void reset(){
		index = 0;
	}
	
	public void setStep (int stepNum) {
	    index = stepNum;
	}
	
	public JSONObject run(JSONObject in){
		if (in.has("communicativeFunction")){
			try {
				if (in.getString("communicativeFunction").equals("getNextStep")){
                    if (index < taskSteps.length()){
						JSONObject temp = taskSteps.getJSONObject(index++);
						temp.put("fromModule", "stm");
						return temp;
					} else {
						JSONObject temp = new JSONObject();
						temp.put("stepId", "999");
						temp.put("fromModule", "stm");
						return temp;
					}
				}
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		return null;
	}
	
	
	private Document parse(URL url) throws DocumentException {
        SAXReader reader = new SAXReader();
        Document document = reader.read(url);
        return document;
	}
	
	private void getScenario1Task(Document scriptDoc) throws JSONException {
		Element root = scriptDoc.getRootElement();
		for (Iterator<Element> i = root.elementIterator(); i.hasNext(); ) {
            Element element = (Element) i.next();
            if (element.getName().equals("activities")){
            	getActivities(element);
            }
		}
		
	}


	private void getActivities(Element root) throws JSONException {
		for (Iterator<Element> i = root.elementIterator(); i.hasNext(); ) {
            Element element = (Element) i.next();
            if (element.getName().equals("steps")){
            	getSteps(element);
            }
		}
		
	}


	private void getSteps(Element root) throws JSONException {
		JSONObject step = new JSONObject();
		for (Iterator<Attribute> j = root.attributeIterator(); j.hasNext();){
			Attribute e = (Attribute) j.next();
			if (e.getName().equals("id")){
				step.put("stepId", e.getValue());
				System.out.println("Stepid:" + e.getValue());
			} else if (e.getName().equals ("time-out")) {
			    step.put("time-out", Double.valueOf (e.getValue ()));
			}
     	}
		step.put("tool-required", "false");
		step.put("tool", "null");
		step.put("info-on-completion", "false");
		step.put("objectPlacementTask", "false");
		
        
		for (Iterator<Element> i = root.elementIterator(); i.hasNext(); ) {
            Element e = (Element) i.next();
            String name = e.getName();
            if (name.equals("scenario-name")){
				step.put(name, e.getStringValue());
			} 
            else if (name.equals("distance")){
				step.put(name, e.getStringValue());
			} else if (name.equals("direction")){
				step.put(name, e.getStringValue());
			} else if (name.equals("direction-required") && !e.getStringValue().equals("")){
				step.put(name, e.getStringValue());
			} else if (name.equals("distance-required") && !e.getStringValue().equals("")){
				try {
					step.put(name, Double.valueOf(e.getStringValue()));
				} catch (NumberFormatException ex){
					step.put(name, 0.0);
				}
				
			} else if (name.equals("symbol-name-required") && !e.getStringValue().equals("")){
				step.put(name, e.getStringValue());
			} else if (name.equals("symbol")){
				step.put(name, e.getStringValue());
			} else if (name.equals("info-on-completion") && !e.getStringValue().equals("")){
				step.put(name, e.getStringValue());
			} else if (name.equals("info-text") && !e.getStringValue().equals("")){
				step.put(name, e.getStringValue());
			} else if (name.equals("step-speech")) {
				step.put(name, e.getStringValue());
				if (e.getStringValue().contains("objectPlacement:")){
					step.put("objectPlacementTask", "true");
				}
			} else if (name.equals("tool")) {
				step.put(name, e.getStringValue());
				step.put("tool-required", "true");
			}
			else if (name.equals("name")){
				if (e.getStringValue().toLowerCase().contains("km")){
					step.put("distance-metric", "km");
				} else {
					step.put("distance-metric", "m");
				}
			}
            
		}
		taskSteps.put(step);
		// logger.info(step.toString());
	}

}