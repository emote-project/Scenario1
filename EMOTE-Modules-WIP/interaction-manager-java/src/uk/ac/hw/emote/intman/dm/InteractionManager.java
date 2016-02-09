package uk.ac.hw.emote.intman.dm;

import java.awt.Point;
import java.io.File;
import java.io.IOException;
import java.util.Collections;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;
import org.eclipse.jetty.util.log.Log;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONTokener;

import uk.ac.hw.emote.intman.Communication;
import emote.ds.im.DialogueManager;
import emote.scenario1.Scenario1TaskManager;
import emote.scenario1.UtteranceGen;

public class InteractionManager {

	private static InteractionManager _inst = new InteractionManager();

	private InteractionManager() {
		PropertyConfigurator.configure("log4j.properties");
		// Singleton
	}

	public static InteractionManager getInstance() {
		return _inst;
	}

	Logger logger = Logger.getLogger(InteractionManager.class.getName());
	
	private String dialogueScriptFile; //where all the dialogue scripts are..
	
	private String scenarioFile;
	private String scenarioName;
	
	private File scriptsDir, scenariosDir;
	
	private Integer startStep;
	private String language;
	private String recapSkill;
	
	public String lastUttId;
	
	public void setLastUtteranceId(String id){
		lastUttId = id;
		dm.setDStateVariable("lastUtteranceId", id);
	}
	
	public void setScriptsDir(File xmlDir) {
		this.scriptsDir = xmlDir;
		try {
			logger.info(
					"All dscript files will be loaded from "
							+ scriptsDir.getCanonicalPath());
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	public void setDialogueScriptFile(String dialogueScriptFile) {
		this.dialogueScriptFile = dialogueScriptFile;
	}

	public void setScenariosDir(File scenariosDir) {
		this.scenariosDir = scenariosDir;
		try {
			logger.info(
					"All scenario files will be loaded from "
							+ scenariosDir.getCanonicalPath());
		} catch (IOException e) {
			e.printStackTrace();
		}
		
	}
	
	public void setStartStep(Integer startStep) {
		this.startStep = startStep;
		logger.info("Scenario 1 will start at step " + startStep);
	}

	// Srini's stuff
	private DialogueManager dm;
	private String currentUtteranceId;
	private Scenario1TaskManager stm;
	//private UtteranceGen ug;
	private JSONArray utteranceQueue;
	private Boolean interactionOngoing;
	// Hard-coded points for Scenario 2
	private List<Point> points;

	public void stop() {
		
		Communication.getSpeechPublisher().CancelUtterance (lastUttId);
		
		dm.setDStateVariable("interactionStatus", "stopped");
		dm.displayDialogueState();
		dm.close();
		dm = null;
		interactionOngoing = false;
	}
	
	public void init(String startInfo) {
		interactionOngoing = true;
		
		// Set up the list of points
		points = new LinkedList<Point>();
		points.add(new Point(3, 1));
		points.add(new Point(5, 1));
		Collections.shuffle(points);

		logger.info ( "start message info: " + startInfo);
		String studentName = "test";
		Boolean isEmpathic = true;
		Boolean isFirstSession = false;
		int studentId = 0;
		String studentGender = "M";
		language = "ENG";
		
		//parsing the start message
		try {
			JSONObject startInput = new JSONObject(new JSONTokener(startInfo));
			logger.info ( "start message decoded: " + startInput.toString());
			JSONArray j = startInput.getJSONArray("Students");
			
			
			if (j.length() > 0){
				JSONObject s1 = j.getJSONObject(0);
				studentName = s1.getString("firstName");
				studentId = s1.getInt("thalamusLearnerId");
				studentGender = s1.getString("sex");
			}
			isEmpathic = startInput.getBoolean("IsEmpathic");
			logger.info ( "IsEmpathic:" + isEmpathic);
			int sessionNumber = startInput.getInt("SessionId");
			if (sessionNumber == 1){
				isFirstSession = true;
			}
			
			
			this.scenariosDir = new File(".\\\\S1ScenarioXMLFiles");
			scenarioName = startInput.getString("ScenarioXmlName");
			scenarioFile = scenarioName + ".xml";
			
			int l = startInput.getInt("Language");
			if (l == 2){
				language = "SWE";
			} else if (l==1){
				language = "ENG";
			} else {
				language = "POR";
			}
			logger.info ( "Language set to:" + language);
			
			Translator.getInstance().init();
			lastUttId = null;
			
			logger.info ( "Trying to load scenario:" + scenarioFile);
		} catch (JSONException e) {
			e.printStackTrace();
		}
		
		if (scenarioFile != null) {
			stm = new Scenario1TaskManager(scenariosDir, scenarioFile);
		}
		
		// Set up all of Srini's components
		dm = new DialogueManager(scriptsDir, dialogueScriptFile);
		dm.reset();
		
		dm.setDStateVariable("interactionStatus", "started");
		dm.setDStateVariable("scenarioName", scenarioName);
		dm.setDStateVariable("learnerName", studentName);
		dm.setDStateVariable("learnerGender", studentGender); //this info is not used in IM.. may be required in Skene for utterance generation
		dm.setDStateVariable("learnerId", String.valueOf(studentId));
		
		if (recapSkill != null){
			dm.setDStateVariable("lastSessionSkill", recapSkill);
		} else {
			dm.setDStateVariable("lastSessionSkill", "null");
		}
		
		if (isEmpathic){
			dm.setDStateVariable("empathicTutor", "true");
		} else {
			dm.setDStateVariable("empathicTutor", "false");
		}
		
		if (isFirstSession){
			dm.setDStateVariable("notFirstSession", "false");
		} else {
			dm.setDStateVariable("notFirstSession", "true");
		}
		
		
		currentUtteranceId = "null";
		
		//ug = new UtteranceGen(dir, utteranceScript);
		
		
		if (stm != null) {
			stm.reset();
			startStep = 0;
			if (startStep != null) {
				stm.setStep(startStep);
			}
		}
		
		
		if (startStep != null) {
            logger.info ("Sending StartStep to map:" + startStep + "");
            Communication.getMapPublisher ().StartStep (startStep);
            try {
                Thread.sleep (2000);
            } catch (InterruptedException e) {
            }
        }
		//ug.reset();
	}

	public String getCurrentUtteranceId(){
		return currentUtteranceId;
	}
	
	public void setCurrentUtteranceId(String id) {
		currentUtteranceId = id;
	}

	
	public void updateInput(JSONObject systemInput) {
		// TODO Auto-generated method stub
		if (dm == null){
			logger.info("No active DM!!!!!");
			return;
		}
		
		synchronized(dm) {
			try {
				logger.info("DM Input: " + systemInput.toString(2));
				dm.updateInputVariables(systemInput);
				dm.executeUpdateRules();
				dm.displayDialogueState();
			} catch (JSONException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
	
	// TODO change to return List<Utterance>
	public Utterance processInput(JSONObject systemInput) throws JSONException {
		if (dm == null){
			logger.info("No active DM!!!!!");
			return null;
		}
		
		synchronized(dm) {
			TurnTakingManager.getInstance().cancelTimeout();
			
			if (interactionOngoing == false){
				return null;
			}
			
			while (dm.isRunning()){
				try {
					Thread.sleep(100);
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
			}
			
			try {
				Utterance nextUtterance = new Utterance();
	
				while (true) {
					logger.info("DM Input: " + systemInput.toString(2));
					JSONArray resultArray = null;
					
					boolean dialogueActionsInQueue = false; 
					if (systemInput.has("fromModule") && systemInput.getString("fromModule").equals("tts")){
						if (systemInput.has("utteranceStatus") && systemInput.getString("utteranceStatus").equals("delivered")
								&& utteranceQueue != null && utteranceQueue.length() > 0){
							logger.info ("Utterance queue not empty!!!!");
							if (systemInput.getString("utteranceId").equals(lastUttId)){
								logger.info ("Utterance: " + lastUttId +" finished. Trying to retrieve next DA from queue");
								//if utterance is finished and the queue has more utterances, use them..
								resultArray = new JSONArray();
								for (int i = 0; i < utteranceQueue.length(); i++){
									resultArray.put(utteranceQueue.getJSONObject(i));
									dialogueActionsInQueue = true; 
									logger.info ("Retrieved next DA from queue");
								}
							} else {
								logger.info ("Utterance: " + systemInput.getString("utteranceId") +" finished. Ignoring utterance finished message."
										+ "Waiting for " + lastUttId + " to finish.");
								return null;
							}
						}
					}
					
					//if there are no DA is queue.. then send systemInput to DM..
					if (!dialogueActionsInQueue){
						resultArray = dm.run(systemInput);
						
						//lets check if there is a presenttask in the queue
						for (int i = 0; i < resultArray.length(); i++){
							JSONObject result = resultArray.getJSONObject(i);
							if (result.has("communicativeFunction")){
								if (result.getString("communicativeFunction").equals("presentTask")){
									Integer stepId = Integer.valueOf(result.getString("currentStepId"));
									if (stepId > 0 && stepId < 999){
										logger.info ("Sending StartNextStep(" + stepId + ")");
										Communication.getMapPublisher ().StartNextStep();
									}
								}
							} 
						}
					} else {
						logger.info ("DM not consulted");
					}
					
					logger.info("DM Output: " + resultArray.toString(2));
					
					if (resultArray.length() == 0){ return null;}
					
					else {
						JSONObject result = resultArray.getJSONObject(0);
						
						utteranceQueue = new JSONArray();
						
						if (resultArray.length() > 1){
							for (int i = 1; i < resultArray.length(); i++){
								utteranceQueue.put(resultArray.getJSONObject(i));
								logger.info("Utterance Queue: " + utteranceQueue.toString());
							}
						} 
						
						/*
						if (result.has("communicativeFunction")){
							if (result.getString("communicativeFunction").equals("presentTask")){
								Integer stepId = Integer.valueOf(result.getString("currentStepId"));
								if (stepId > 0 && stepId < 999){
									logger.info ("Sending StartNextStep(" + stepId + ")");
									Communication.getMapPublisher ().StartNextStep();
								}
							}
						} 
						*/
						
						// Check which module to send it to now
						if (result.has("toModule")) {
							// Next step in Scenario 1
							if ("stm".equals(result.get("toModule"))) {
								// Get the next step from the scenario task manager
								// and
								// store it for future use
								JSONObject stmResult = stm.run(result);
								logger.info(
										"STM Result: " + stmResult.toString(2));
								if (stmResult.has("stepId")) {
									// Probably not necessary any more
									nextUtterance.stepId = stmResult
											.getInt("stepId");
								}
								// Loop
								systemInput = stmResult;
	
							} else if ("skene".equals(result.get("toModule"))) {
								logger.info("Processing Skene message");
								String commFunction = result
										.getString("communicativeFunction");
								if ("executeGameAction".equals(commFunction)) {
									// Is it an actual action? If so, send it
									// directly
									// to Enercities
									String gameAction = result
											.getString("gameAction");
									if ("confirmConstruction".equals(gameAction)) {
										String structure = result
												.getString("gameStructure");
										int posX = result.getInt("gamePositionX");
										int posY = result.getInt("gamePositionY");
										logger.info(
												"Construction location: (" + posX
														+ "," + posY + ")");
	
										if (points != null && !points.isEmpty()) {
											Point point = points.remove(0);
											logger.info(
													"Using hard-coded point location "
															+ point);
											posX = point.x;
											posY = point.y;
										} else {
											logger
													.info("No hard-coded points left; using original location");
										}
	
										logger.info(
												"Constructing a " + structure
														+ " at (" + posX + ","
														+ posY + ")");
										Communication.getEnercitiesPublisher()
												.ConfirmConstruction(structure,
														posX, posY);
	
										nextUtterance.content = "I built a "
												+ structure.replaceAll("_", " ");
										nextUtterance.standalone = true;
										return nextUtterance;
									}
								} else {
									// Create an utterance for Skene (total fake for
									// now)
									if ("informTurnChange".equals(commFunction)) {
										String player = result
												.getString("gameTurnHolder");
										nextUtterance.content = "Now it's the "
												+ player + "'s turn!";
									} else {
										nextUtterance.content = commFunction;
									}
									nextUtterance.standalone = false;
									return nextUtterance;
								}
	
							} else if ("user".equals(result.get("toModule"))) {
								
								// Extract necessary tags
								Map<String, String> tags = new HashMap<String, String>();
								tags.put("student", result.getString("learnerName"));
								
								// The rest may or may not exist ...
								if (result.has("correctDirection")) {
									tags.put("direction", result.getString("correctDirection"));
								}
								if (result.has("correctDistance")) {
									String temp = String.valueOf(result.getDouble("correctDistance"));
									temp = temp.replaceAll("\\.0", "");
									if (result.has("distanceMetric") && result.getString("distanceMetric").equals("km")){
										temp = temp + " kilometers";	
									} else {
										temp = temp + " meters";
									}
									tags.put("distance", temp);
								}
								if (result.has("correctTool")) {
									tags.put("tool", result.getString("correctTool"));
								}
								if (result.has("correctSymbol")) {
									tags.put("symbol", result.getString("correctSymbol"));
								}
								if (result.has("previousSymbol")) {
									tags.put("symbol2", result.getString("previousSymbol"));
								}
								if (result.has("skill")) {
									tags.put("skill", result.getString("skill"));
								}
								
								
								
								// Standalone utterance
								if (result.has("utterance")) {
									/* nextUtterance.content = result
											.getString("utterance");*/
									logger.info ("***** Copying utterance into cf: " + result.getString("utterance"));
									result.put ("communicativeFunction", result.getString("utterance"));
									/*
									JSONObject r2 = ug.processUtterance(result);
									nextUtterance.content = r2.getString("utterance");
									*/
								} 
								/*else*/ if (result.has("communicativeFunction")) {
									String cf = result.getString("communicativeFunction");
									if (cf.equals("taskOutro")){
										String scenarioName2 = "null";
										if (result.has("scenarioName")){
											scenarioName2 = result.getString("scenarioName");
										} else {
											scenarioName2 = this.scenarioName;
										}
										scenarioName2 = scenarioName2.replaceAll("_", "");
										scenarioName2 = scenarioName2.toLowerCase();
										cf = cf + ":" + scenarioName2;
										result.put("communicativeFunction", cf);
									}
									else if (cf.equals("clueSpeech")){
										int clueNo = result.getInt("clueNumber");
										String scenarioName2 = "null";
										if (result.has("scenarioName")){
											scenarioName2 = result.getString("scenarioName");
										} else {
											scenarioName2 = this.scenarioName;
										}
										scenarioName2 = scenarioName2.replaceAll("_", "");
										scenarioName2 = scenarioName2.toLowerCase();
										cf = cf + ":" + scenarioName2 + "Clue" + clueNo;
										result.put("communicativeFunction", cf);
									}
									
									//sending the utterance to skene
									//ug uses the utterance library in XLS format.. to find the right utterance for the given CF
									// JSONObject ugResult = ug.run(result);
									// nextUtterance.content = (ug.processUtterance(ugResult)).getString("utterance");
									
									//sending the cf to skene
									//cf is of format cat:subCat.. which are split in Utterance.java
									nextUtterance.cf = result.getString("communicativeFunction");
									nextUtterance.tags = tags;
									nextUtterance.language = this.language;
									
									logger.info ("****** SET CF to " + nextUtterance.cf);
									nextUtterance.content = null;
									
									
								} else {
									nextUtterance.timeout = 10;
									nextUtterance.standalone = true;
									return nextUtterance;
								}
								if (result.has("time-out")) {
									nextUtterance.timeout = result
											.getInt("time-out");
								}
	
								nextUtterance.standalone = true;
								return nextUtterance;
							} else {
								return null;
							}
						} else {
							return null;
						}
					}
				}
			
			} catch (RuntimeException ex) {
				ex.printStackTrace();
				logger.warn("Some exception:" + ex);
			}
			return null;
		}
	}

	

	public void setRecapInfo(String learnerStateInfo_learnerState) {
		//parsing the learnerStateInfo message
		try {
			JSONObject startInput = new JSONObject(new JSONTokener(learnerStateInfo_learnerState));
			logger.info ( "LearnerStateInfo message decoded: " + startInput.toString());
			JSONArray j = startInput.getJSONArray("competencyItems");
			Double max = 0.0;
			String maxCompetency = "null";
			for (int i=0; i<j.length();i++){
				JSONObject c1 = j.getJSONObject(0);
				Double d = c1.getDouble("comptencyValue");
				if (d >= max){
					max = d;
					maxCompetency = c1.getString("competencyType");
				}
			}
			if (maxCompetency.equals("10")){
				maxCompetency = "distance";
			}
			else if (maxCompetency.equals("11")){
				maxCompetency = "direction";
			}
			else if (maxCompetency.equals("12")){
				maxCompetency = "symbol";
			}
			
			do {
				try {
					Thread.sleep(1000);
					logger.info ( "LearnerStateInfo message waiting for dm to be initialized..");
				} catch (Exception e) {
					e.printStackTrace();
				}
			} while (dm == null);
			
			synchronized(dm){
				recapSkill = maxCompetency;
				if (dm != null && recapSkill != null){
					dm.setDStateVariable("lastSessionSkill", recapSkill);
				} else {
					dm.setDStateVariable("lastSessionSkill", "null");
				}
			}
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}

	public String getLastUtteranceId() {
		return lastUttId;
	}


	

}
