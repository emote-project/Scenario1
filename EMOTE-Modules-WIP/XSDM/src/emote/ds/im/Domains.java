package emote.ds.im;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;



public class Domains{
	DialogueState d;
	Input iv;
	Output ov;
	Metric mt;

	Logger logger = Logger.getLogger(Domains.class.getName());
	
	
	public Domains(){
		PropertyConfigurator.configure("log4j.properties");
		d = new DialogueState();
		iv = new Input();
		ov = new Output();
		mt = new Metric();
	}
	
}