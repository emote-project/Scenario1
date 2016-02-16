package emote.scenario1;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Hashtable;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;
import org.apache.poi.hssf.usermodel.HSSFCell;
import org.apache.poi.hssf.usermodel.HSSFRow;
import org.apache.poi.hssf.usermodel.HSSFSheet;
import org.apache.poi.hssf.usermodel.HSSFWorkbook;
import org.json.JSONException;
import org.json.JSONObject;


public class UtteranceGen {
	
	private class Realisation{
		String cf;
		String utterance;
		
		Realisation(String a, String b){
			cf = a;
			utterance = b;
		}
	}
	
	Logger logger = Logger.getLogger(UtteranceGen.class.getName());
	
	ArrayList<Realisation> utt;
	
	public UtteranceGen(File dir, String filename){
		PropertyConfigurator.configure("log4j.properties");
		
		File file = new File (dir, filename);
		
		utt = new ArrayList<Realisation>();
		loadUtterancesFromLibrary(file);
	}
	
	
	private void loadUtterancesFromLibrary(File f){
		try {
			FileInputStream fileInputStream = new FileInputStream(f);
			HSSFWorkbook workbook = new HSSFWorkbook(fileInputStream);
			HSSFSheet worksheet = workbook.getSheet("Utterances");
			HSSFRow row1 = worksheet.getRow(0);
			HSSFCell cellA1 = row1.getCell(0);
			String a1Val = cellA1.getStringCellValue();
			
			HSSFCell cellB1 = row1.getCell(1);
			String b1Val = cellB1.getStringCellValue();
			
			int l = worksheet.getLastRowNum();
			
			logger.info("Size of utterance library: " + l);
			
			for (int i=0; i < l; i++){
				HSSFRow row = worksheet.getRow(i);
				HSSFCell cellA = row.getCell(0);
				String aVal = "";
				if (cellA == null){
					aVal = "";
				} else {
					aVal = cellA.getStringCellValue();
				}
				
				
				HSSFCell cellB = row.getCell(1);
				String bVal = cellB.getStringCellValue();
				utt.add(new Realisation(bVal, aVal));
				
				logger.info("" + i + ";" + aVal + ";" + bVal);
				
			}
			
			workbook.close();
			
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	
	private String[] getAllRealisations(String cf){
		ArrayList<String> out = new ArrayList<String>();
		for (int i=0; i < utt.size(); i++){
			Realisation r = utt.get(i);
			if (r.cf.equals(cf)){
				out.add(r.utterance);
			}
		}
		
		String [] outSA = out.toArray(new String[out.size()]);
		return outSA;
	}
	
	
	public JSONObject processUtterance(JSONObject in){
		JSONObject out = in;
		
		if (in.has("utterance")){
			try {
				String utt = in.getString("utterance");
				if (utt.contains("/student/")){
					String name = "friend";
					if (in.has("learnerName") && !in.getString("learnerName").equals("null")){
						name = in.getString("learnerName");
					}
					utt = utt.replaceAll("/student/", name);
				}
				in.put("utterance", utt);
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		return out;
	}
	
	public JSONObject run(JSONObject in){
		JSONObject out = null;
		if (in.has("communicativeFunction")){
			out = new JSONObject();
			
			try {
				
				out.put("fromModule", "uttGen");
				
				
				String[] r = getAllRealisations(in.getString("communicativeFunction"));
				if (r.length > 0){
					String resp = generateRandomly(r);
					out.put("utterance", resp);
				} else {
					out.put("utterance", in.getString("communicativeFunction") + " needs an utterance");
				}
				
				
				/* 
				if (in.getString("communicativeFunction").equals("pump:tool")){
					
					if (in.getString("correctTool").equals("mapKey")){
						out.put("utterance", "Which tool helps you with symbols on maps?");
					} 
					else if (in.getString("correctTool").equals("compass")){
						out.put("utterance", "Which tool should you use for directions?");
					} 
					else if (in.getString("correctTool").equals("distance")){
						out.put("utterance", "What tool can you use to measure distance?");
					} 
					
				} 
				
				else if (in.getString("communicativeFunction").equals("pump:direction")){
					
					String[] r = new String[]{"So which direction could that be?", 
							"Which way is /direction/?", 
							"Which way should you go?",
							"Which direction is /direction/?", 
							"Which direction should we go?", 
							"Where could /direction/ of your current position be?"};
					String resp = generateRandomly(r);
					
					out.put("utterance", resp);
					
				} 
				
				else if (in.getString("communicativeFunction").equals("pump:symbol")){
					
					
					
					String[] r = new String[]{"What could a symbol of a /symbol/ look like on a map?", 
							"Is there a tool that you can use to figure this out?", 
							"Can you see the /symbol/ on the map?", "Can you locate the /symbol/ on the map?", 
							"Where is the /symbol/ on the map?"};
					String resp = generateRandomly(r);
					
					
					out.put("utterance", resp);
					
					
				} 
				
				else if (in.getString("communicativeFunction").equals("pump:distance")){
					
					
					
					String[] r = new String[]{"How far is /distance/?", 
							"Is there a tool that you can use to measure the distance?", 
							"How many meters are there in /distance/?"};
					String resp = generateRandomly(r);
					
					
					out.put("utterance", resp);
					
					
				}
				 
				else if (in.getString("communicativeFunction").equals("veryPositiveFeedback")){
					
					String[] r = new String[]{"<GAZE(student)> <ANIMATE(enthusiasm)> Awesome!", 
							"<GAZE(student)> <ANIMATE(enthusiasm)> Super!", 
							"<GAZE(student)> <ANIMATE(enthusiasm)> Way to go!",
							"<GAZE(student)> <ANIMATE(enthusiasm)> Excellent, /student/!",
							"<GAZE(student)> <ANIMATE(enthusiasm)> This is great!",
							"<GAZE(student)> <ANIMATE(enthusiasm)> You got it right! <HEADNOD(3)>"};
										
					out.put("utterance", generateRandomly(r));
					
				}
				else if (in.getString("communicativeFunction").equals("positiveFeedback")){
					
					String[] r = new String[]{"<GAZE(student)> <ANIMATE(happy)> Very good. <HEADNOD(1)>", 
							"<GAZE(student)> <ANIMATE(happy)> Good job. <HEADNOD(1)>", 
							"<GAZE(student)> <ANIMATE(happy)> Great work. <HEADNOD(1)>",
							"<GAZE(student)> <ANIMATE(happy)> Exactly.",
							"<GAZE(student)> <ANIMATE(happy)> Precisely.",
							"<GAZE(student)> <HEADNOD(2)> You got <ANIMATE(pointToOther)> that right."};
					out.put("utterance", generateRandomly(r));
					
				}
				else if (in.getString("communicativeFunction").equals("rebound")){
					
					String[] r = new String[]{"Would you like to try again?", 
							"Try again.", 
							"Let's go again.",
							"Let's try this again.",
							"Let's see if we can get this now."};
					out.put("utterance", generateRandomly(r));
					
				}
				else if (in.getString("communicativeFunction").equals("requestion")){
					
					if (in.has("utterance")){
						//we use utterance from script itself for tool strategy requestions..
						out.put("utterance", in.getString("utterance"));
						
					} else {
						
						String[] r = new String[]{
								"So, we were supposed to find a /symbol/ symbol /distance/ meters /direction/. Where could that be?",
								"Remember that we were looking for a /symbol/ symbol /distance/ meters /direction/ of where you were.",
								"We were supposed to locate the /symbol/ symbol /distance/ meters /direction/ of the your current location. So where should we go?",
								"/student/ Remember that we should go towards the /direction/ /distance/ meters from where you are, and we might spot the /symbol/ symbol. Can you find it?",
								"Ok, so we were supposed to head /distance/ meters /direction/, until we reach a /symbol/ symbol. Where could that be?",
								"Ok, so we are going towards the /direction/ for /distance/ meters, and looking for a /symbol/ symbol somewhere in that area. Do you see it?",
								"Remember that we were looking for the /symbol/ symbol /distance/ meters /direction/.",
								"So we are supposed to go /distance/ meters in the following direction: /direction/. Can you find the /symbol/ symbol somewhere around there?",
						};
						out.put("utterance", generateRandomly(r));
					}
				} 
					else if (in.getString("communicativeFunction").equals("tellAJoke")){
					
					//out.put("utterance", "Have you ever done orienteering? Yes. Well I wonder if this might help you with your navigation for such situations. Too bad I cannot explore my geography skills through orienteering. That is one of the downsides of being attached to a table. <laugh>");
						out.put("utterance", "Have you ever done orienteering? Yes. Well I wonder if this might help you with your navigation for such situations. Too bad I cannot explore my geography skills through orienteering. That is one of the downsides of being attached to a table.");
				}
				else if (in.getString("communicativeFunction").equals("focusStudentAttention:symbol")){
					
					out.put("utterance", "I suggest you look at the Map Key again.");
					
				}
				else if (in.getString("communicativeFunction").equals("focusStudentAttention:direction")){
					
					out.put("utterance", "Check the compass again.");
					
				}
				else if (in.getString("communicativeFunction").equals("focusStudentAttention:distance")){
					
					out.put("utterance", "How about measuring again?");
					
				}
				else if (in.getString("communicativeFunction").equals("almost:symbol")){
					
					out.put("utterance", "<GAZE(student)> <HEADNOD(2)> You figured out the cardinal direction and distance here, but the symbol is not quite right. <GAZE(clicks)>");
					
				}
				else if (in.getString("communicativeFunction").equals("almost:direction")){
					
					out.put("utterance", "<GAZE(student)> <HEADNOD(2)> You figured out the right symbol and distance here, but the direction is not quite right. <GAZE(clicks)>");
					
				}
				else if (in.getString("communicativeFunction").equals("almost:distance")){
					
					out.put("utterance", "<GAZE(student)> <HEADNOD(2)> You figured out the right symbol and the cardinal direction here, but the distance is not quite right. <GAZE(clicks)>");
					
				}
				else if (in.getString("communicativeFunction").equals("elaboration:tool")){
					
					String utt = "null";
					if (in.has("correctTool")){
						if (in.getString("correctTool").equals("mapKey")){
							utt = "You can use the Map Key tool to figure out the symbol for different features such as telephone booths, information centers, etc. Map symbols are either red or blue on this map.";
						} else if (in.getString("correctTool").equals("compass")){
							utt = "You can use the compass tool to figure out the cardinal points. This will help you find the way. What you should keep in mind then, is that N means North. S means South. W means West. And E means East.";
						} else if (in.getString("correctTool").equals("distance")){
							utt = "You can use the distance tool to measure the distance between these two points. You have to click where you want to measure from and then double click where you want to measure  to. You can see how far you've measured on the screen.";
						} else {
							utt = "You can use the tools below to measure distance and identify symbols and directions.";
						}
					}
					out.put("utterance", utt);
					
				}
				else if (in.getString("communicativeFunction").equals("elaboration:distance")){
					String[] r = new String[]{"In order to figure out how far /distance/ is on a  scaled down map such as this, you should look at the scale of the map located on your left side of the screen. The measuring tool will then allow you to determine specific distances.",
							"To measure the distance we should use the distance tool at the bottom right side of the screen.",
							"You can use the distance tool to measure the distance between these two points. You have to click where you want to measure from and then double click where you want to measure  to. You can see how far you've measured on the screen. "};
					out.put("utterance", generateRandomly(r));
				}
				else if (in.getString("communicativeFunction").equals("elaboration:direction")){
					String[] r = new String[]{"You have four main  cardinal points:  North, South, East, and West."};
					out.put("utterance", generateRandomly(r));
				}
				else if (in.getString("communicativeFunction").equals("elaboration:symbol")){
					String[] r = new String[]{"The /symbol/ has a specific map symbol associated with it. If you look at the map key tool, you can find the symbol of the /symbol/."};
					out.put("utterance", generateRandomly(r));
				}
				
				else if (in.getString("communicativeFunction").equals("summarize")){
					String[] r = new String[]{"Ok, so now you managed to find the /symbol/ symbol /distance/ meters /direction/ of your previous location.",
									"So you managed to find the /symbol/ symbol /distance/ meters /direction/ of your initial location.",
									"You got it right!"};
					out.put("utterance", generateRandomly(r));
				}
				else if (in.getString("communicativeFunction").equals("hint:direction")){
					
					out.put("utterance", "<GAZEANDWAVE(rightAnswer)> It is more towards this way.");
					
				}
				else if (in.getString("communicativeFunction").equals("hint:distance")){
					
					out.put("utterance", "<GAZEANDWAVE(rightAnswer)> It is around here somewhere.");
					
				}
				else if (in.getString("communicativeFunction").equals("hint:symbol")){
					
					out.put("utterance", "<GAZEANDWAVE(rightAnswer)> The symbol is located somewhere over here.");
					
				}
				else if (in.getString("communicativeFunction").equals("greetUser")){
					
					String[] r = new String[]{"<GAZE(student)> <HEADNOD(2)> Hello there, /student/!", 
							"<GAZE(student)> <HEADNOD(2)> Hi there, /student/!"};
					out.put("utterance", generateRandomly(r));
					
				}
				else if (in.getString("communicativeFunction").equals("askReady")){
					
					String[] r = new String[]{"<GAZE(student)> <HEADNOD(1)> Ok! Lets get this started."};
					out.put("utterance", generateRandomly(r));
					
				}
				else if (in.getString("communicativeFunction").equals("recap")){
					
					String[] r = new String[]{"<GAZE(student)> You did well in directions last time. Lets work on your distance skills this time."};
					out.put("utterance", generateRandomly(r));
					
				}
				else if (in.getString("communicativeFunction").equals("taskIntro")){
					
					String[] r = new String[]{"<GAZE(student)> <ANIMATE(enthusiasm)>Today, lets continue to work on your map reading skills."};
					out.put("utterance", generateRandomly(r));
					
				}
				
				else if (in.getString("communicativeFunction").equals("splice")){
					
					String[] r = new String[]{"/student/, don't worry, I can help you out. The answer that we were looking for is right here.  <GAME(rightAnswer)>", 
							"Ok. Let me help you with the answer. Here it is <GAME(rightAnswer)>."};
					out.put("utterance", generateRandomly(r));
					
				}
				else if (in.getString("communicativeFunction").equals("smallTalk")){
					
					String[] r = new String[]{"<GAZE(student)> <ANIMATE(happy)> Are you having fun?", 
							"<GAZE(student)> <ANIMATE(pointToOther)> Are you bored?",
							"<GAZE(student)> <ANIMATE(pointToOther)> Do you like geography /student/?",
							"<GAZE(student)> <POINT(student)>Have you ever done orienteering?",
							"<GAZE(student)> <ANIMATE(pointToOther)> Do you know any other robots besides me?",
							"<GAZE(student)> <GLANCE(throughMap)> Do you think this touchtable works well?"};
					out.put("utterance", generateRandomly(r));
					
				}
				else {
					
					out.put("utterance", in.getString("communicativeFunction") + " needs an utterance");
					
				}
				*/
				
				String correctDirection = "null";
				if (in.has("correctDirection")){
					correctDirection = in.getString("correctDirection");
				}
				String correctDistance = "null";
				if (in.has("correctDistance")){
					correctDistance = String.valueOf(in.getDouble("correctDistance")).replace(".0", "");
				}
				
				String correctSymbol = "null";
				if (in.has("correctSymbol")){
					correctSymbol = in.getString("correctSymbol");
				}
				
				String resp = out.getString("utterance");
				if (resp.contains("/direction/")){
					resp = resp.replaceAll("/direction/", correctDirection);
				}
				if (resp.contains("/distance/")){
					resp = resp.replaceAll("/distance/", correctDistance);
				}
				if (resp.contains("/symbol/")){
					resp = resp.replaceAll("/symbol/", correctSymbol);
				}
				
				if (resp.contains("null")){
					logger.info("null detected in utterance:" + resp);
					resp = "There is a null in this utterance!";
				} else {
					logger.info("UttGen:" + resp);
				}
				
				out.put("utterance", resp);
				
				if (in.has("learnerName")){
					out.put("learnerName", in.getString("learnerName"));
				}
				
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		return out;
	}

	private String generateRandomly(String[] responses){
		Double r = Math.random();
		Double step = 1.0 / responses.length;
		Double s1 = step;
		
		for (int i = 0; i < responses.length; i++){
			if (r < s1){ return responses[i];}
			s1 += step;
		}
		if (responses.length > 0){
			return responses[0];
		} else {
			return "null";
		}
	}
	
	
	
	
	public void reset() {
		// TODO Auto-generated method stub
		
	}
}
