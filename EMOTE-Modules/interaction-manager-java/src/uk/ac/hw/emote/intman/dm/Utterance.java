package uk.ac.hw.emote.intman.dm;

import java.util.Arrays;
import java.util.Iterator;
import java.util.Map;
import java.util.concurrent.atomic.AtomicReference;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;
import org.eclipse.jetty.util.log.Log;
import org.json.JSONException;
import org.json.JSONObject;

import redstone.xmlrpc.XmlRpcArray;
import uk.ac.hw.emote.intman.Communication;

public class Utterance {

    public enum UtteranceState {
        Queued, Waiting, Started, Finished, Cancelled
    }
    
    Logger logger = Logger.getLogger(Utterance.class.getName());
    
    public String language;
    public String cf;
    public String content;
    public Integer stepId;
    public double timeout;
    public String id, lastUttId;
    public AtomicReference<UtteranceState> uttState;
    public boolean standalone;
	public Map<String, String> tags;

    public Utterance () {
    	PropertyConfigurator.configure("log4j.properties");
        this.uttState = new AtomicReference<UtteranceState> (UtteranceState.Queued);
        this.id = "null";
        //this.lastUttId = "null";
    }

    public void perform () {
        UtteranceState oldState;
        if ( (oldState = uttState.getAndSet (UtteranceState.Waiting)) != UtteranceState.Queued) {
            logger.warn ("Unexpected previous utterance state " + oldState);
        }

        Runnable doPerform = new Runnable () {

            @SuppressWarnings ("unchecked")
            @Override
            public void run () {
            	/*
                if (stepId != null && stepId != 999 ) {
                    logger.info ("Sending StartStep(" + stepId + ")");
                    Communication.getMapPublisher ().StartStep (stepId);
                    try {
                        Thread.sleep (2000);
                    } catch (InterruptedException e) {
                    }
                }
				*/
                  
                if (content != null){
                	
                	 // Disable the map after changing the step
                    // Communication.getMapPublisher ().BlockUI ();
                 
                	
	                logger.info ("Invoking PerformUtterance('" + content + "')");
	                
	                // Temporary hacks for initial Scenario 2 version
	                if ("explainGameAction".equals (content)) {
	                	/*
	                    XmlRpcArray dummy = new XmlRpcArray ();
	                    dummy.add ("hello");
	                    Communication.getSpeechPublisher ().PerformUtteranceFromLibrary ("",
	                            "turntaking", "robot", dummy, dummy);
	                            */
	                	content = "This will help our city.";
	                }
	                Communication.getSpeechPublisher ().CancelUtterance("");
	                
	                Communication.getSpeechPublisher ().PerformUtterance ("", content, "");
	                
	                
                } else if (cf != null){
                	// Change to PerformUtteranceFromLibrary
                	String category = "";
	                String subcat = "";

	                if (cf.contains(":")){
                		String[] a = cf.split(":");
                		category = a[0].toLowerCase();
                		subcat = a[1].toLowerCase();
                	} else {
                		category = cf.toLowerCase();
                		subcat = "-";
                	}
	                
	                // category = "pump";
	                // subcat = "direction";
	                
	                XmlRpcArray tagsArray = new XmlRpcArray();
	                XmlRpcArray valuesArray = new XmlRpcArray();
	                
	                for (String key : tags.keySet()) {
                		String label = tags.get(key);
                		if (key.equals("symbol")){
	                		JSONObject tr = Translator.getInstance().translateSymbol(language, label);
	                		if (tr != null){
	                			//logger.info ("TRANSLATION: Found translation for symbol:" + tr.toString());
	                			Iterator<String> k = tr.keys();
	                			while (k.hasNext()){
	                				String k2 = k.next();
	                				try {
										String k2Value = tr.getString(k2);
										tagsArray.add (k2);
					                	valuesArray.add (k2Value);
									} catch (JSONException e) {
										e.printStackTrace();
									}
	                				
	                			}
	                		}
	                	} 
                		else if (key.equals("symbol2")){
                			//logger.info ("TRANSLATION: Finding translation for symbol2:" + label);
	                		JSONObject tr = Translator.getInstance().translateSymbol2(language, label);
	                		if (tr != null){
	                			//logger.info ("TRANSLATION: Found translation for symbol2:" + tr.toString());
	                			Iterator<String> k = tr.keys();
	                			while (k.hasNext()){
	                				String k2 = k.next();
	                				try {
										String k2Value = tr.getString(k2);
										k2 = k2.replaceAll("symbol", "symbol2");
										tagsArray.add (k2);
					                	valuesArray.add (k2Value);
									} catch (JSONException e) {
										e.printStackTrace();
									}
	                				
	                			}
	                		} else {
	                			logger.info ("TRANSLATION: No translation found for symbol:" + label);
	                			tagsArray.add ("/" + key + "/");
			                	valuesArray.add (translate(key, tags.get (key)));
	                		}
	                	} 
                		else if (key.equals("direction")){
	                		JSONObject tr = Translator.getInstance().translateDirection(language, label);
	                		if (tr != null){
	                			//logger.info ("TRANSLATION: Found translation for label:" + tr.toString());
	                			Iterator<String> k = tr.keys();
	                			while (k.hasNext()){
	                				String k2 = k.next();
	                				try {
										String k2Value = tr.getString(k2);
										tagsArray.add (k2);
					                	valuesArray.add (k2Value);
									} catch (JSONException e) {
										e.printStackTrace();
									}
	                				
	                			}
	                		} else {
	                			logger.info ("TRANSLATION: No translation found for direction:" + label);
	                			tagsArray.add ("/" + key + "/");
			                	valuesArray.add (translate(key, tags.get (key)));
	                		}
	                	} 
                		else if (key.equals("tool")){
	                		JSONObject tr = Translator.getInstance().translateTool(language, label);
	                		if (tr != null){
	                			//logger.info ("TRANSLATION: Found translation for tool:" + tr.toString());
	                			Iterator<String> k = tr.keys();
	                			while (k.hasNext()){
	                				String k2 = k.next();
	                				try {
										String k2Value = tr.getString(k2);
										tagsArray.add (k2);
					                	valuesArray.add (k2Value);
									} catch (JSONException e) {
										e.printStackTrace();
									}
	                				
	                			}
	                		} else {
	                			logger.info ("TRANSLATION: No translation found for tool:" + label);
	                			tagsArray.add ("/" + key + "/");
			                	valuesArray.add (translate(key, tags.get (key)));
	                		}
	                	} 
                		else if (key.equals("skill")){
	                		JSONObject tr = Translator.getInstance().translateSkill(language, label);
	                		if (tr != null){
	                			//logger.info ("TRANSLATION: Found translation for skill:" + tr.toString());
	                			Iterator<String> k = tr.keys();
	                			while (k.hasNext()){
	                				String k2 = k.next();
	                				try {
										String k2Value = tr.getString(k2);
										tagsArray.add (k2);
					                	valuesArray.add (k2Value);
									} catch (JSONException e) {
										e.printStackTrace();
									}
	                				
	                			}
	                		} else {
	                			logger.info ("TRANSLATION: No translation found for skill:" + label);
	                			tagsArray.add ("/" + key + "/");
			                	valuesArray.add (translate(key, tags.get (key)));
	                		}
	                	} 
                		else if (key.equals("distance")){
	                		JSONObject tr = Translator.getInstance().translateDistance(language, label);
	                		if (tr != null){
	                			//logger.info ("TRANSLATION: Found translation for distance:" + tr.toString());
	                			Iterator<String> k = tr.keys();
	                			while (k.hasNext()){
	                				String k2 = k.next();
	                				try {
										String k2Value = tr.getString(k2);
										tagsArray.add (k2);
					                	valuesArray.add (k2Value);
									} catch (JSONException e) {
										e.printStackTrace();
									}
	                				
	                			}
	                		} else {
	                			logger.info ("TRANSLATION: No translation found for distance:" + label);
	                			tagsArray.add ("/" + key + "/");
			                	valuesArray.add (translate(key, tags.get (key)));
	                		}
	                	} 
                		else {
		                	tagsArray.add ("/" + key + "/");
		                	valuesArray.add (translate(key, tags.get (key)));
	                	}
	                }
	                id = "U" + System.currentTimeMillis();
	                logger.info ("*** Sending to Skene:: utterance info: " + id + ":" + category + ":" + subcat );
	                logger.info ("*** Tags: " + tags );
	                
	                lastUttId = InteractionManager.getInstance().getLastUtteranceId();
	                if (lastUttId != null){
	                	Communication.getSpeechPublisher().CancelUtterance (lastUttId);
	                }
	                InteractionManager.getInstance().setLastUtteranceId(id);
	                Communication.getSpeechPublisher().PerformUtteranceFromLibrary(id, category, subcat, tagsArray, valuesArray);
	                
                }
            }
        };

        new Thread (doPerform).start ();
    }

    

	
	private String translate(String key, String value){
		if (language.equals("ENG")){
			if (key.equals("tool") && value.equals("distance")){
				return "distance tool";
			} 
			return value;
		} 
		else if (language.equals("SWE")){
			if (value.equals("mapKey")){
				return "symbolkartan";
			}
			if (value.equals("compass")){
				return "kompassen";
			}
			if (value.equals("distance")){
				return "distance tool";
			}
			return value;
		} 
		else if (language.equals("POR")){
			if (value.equals("mapKey")){
				return "symbolkartan";
			}
			if (value.equals("compass")){
				return "kompassen";
			}
			if (value.equals("distance")){
				return "distance tool";
			}
			return value;
		} 
		return "null";
	}
    
    /*
    public void cancel () {
        if (lastUttId.equals("null")) {
            logger.warn ("Not sending CancelUtterance for utterance with no ID");
            uttState.getAndSet (UtteranceState.Cancelled);
        } else {
           //Communication.getSpeechPublisher ().CancelUtterance (lastUttId);
        }
    }
	*/
    
    @Override
    public String toString () {
        String truncContent = (content.length () < 10) ? content
                : (content.substring (0, 10) + "...");
        return "Utterance [content=" + truncContent + ", , stepId=" + stepId + ", timeout="
                + timeout + ", id=" + id + ", uttState=" + uttState + "]";
    }

}
