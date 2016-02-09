package uk.ac.hw.emote.intman;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;

import org.eclipse.jetty.util.log.Log;
import org.kohsuke.args4j.Argument;
import org.kohsuke.args4j.CmdLineException;
import org.kohsuke.args4j.CmdLineParser;
import org.kohsuke.args4j.Option;

import redstone.xmlrpc.XmlRpcException;
import uk.ac.hw.emote.intman.dm.InteractionManager;
import uk.ac.hw.emote.intman.dm.TurnTakingManager;

public class Main {
    private static class Options {
        @Option (name = "-scriptsDir", usage = "Directory containing dialogue script files")
        private String scriptsDir; // = "c:/Users/mef3/Documents/emote/xsdm/xsdm";

        @Option (name = "-scenarioDir", usage = "Scenario file to load")
        private String scenarioDir; // = "c:/Users/mef3/Documents/emote/xsdm/xsdm";

        @Option (name = "-scriptFile", usage = "Scenario 1 script file")
        private String scriptFile; 

        @Option (name = "-startStep", usage = "Start step for Scenario 1")
        private Integer startStep; // = 11;

        @Option (name = "-utteranceFile", usage = "Scenario 1 utterance library XLS file")
        private String utteranceFile; // = "Utterances_S1_NEW_v6.xls";
        
        @Argument
        private List<String> arguments = new ArrayList<String> ();

        @Override
        public String toString () {
            return "Options [scriptsDir=" + scriptsDir + ", scenarioDir=" + scenarioDir + ", scriptFile="
                    + scriptFile + ", startStep=" + startStep + ", utteranceFile="
                            + utteranceFile + ", arguments=" + arguments + "]";
        }
    }

    public static void main (String[] args) {
        if (!parseCommandLine (args)) {
            Log.getLog ().warn ("Not starting IM because all necessary files were not found");
            return;
        }

        // Start the XMLRPC server to receive messages from the rest of the
        // system
        Thread serverThread = Communication.getServerThread ();
        serverThread.start ();

        /*
        // Start the system off
        try {
            Communication.getEmotePublisher ().Start (new Random ().nextInt (), 123);
        } catch (XmlRpcException e1) {
            Log.getLog ().warn ("Couldn't connect to C# client; exiting");
            return;
        }
		*/
        // Initialise the interaction manager
        //TurnTakingManager.getInstance ().init (123, "Srini");
        

        // Just keep going until we are terminated
        while (serverThread.isAlive ()) {
            try {
                Thread.sleep (100);
            } catch (InterruptedException e) {
            }
        }
    }

    private static boolean parseCommandLine (String[] args) {
        Options opts = new Options ();
        CmdLineParser parser = new CmdLineParser (opts);
        try {
            parser.parseArgument (args);
        } catch (CmdLineException e1) {
            Log.getLog ().warn ("Unable to parse command line", e1);
        }

        InteractionManager im = InteractionManager.getInstance ();

        File scriptsDir = null;
        if (opts.scriptsDir != null) {
            scriptsDir = new File (opts.scriptsDir);
        }
        if (scriptsDir == null || !scriptsDir.exists ()) {
            Log.getLog ().warn ("DScript directory " + opts.scriptsDir + " does not exist!");
            return false;
        } else {
            try {
                scriptsDir = new File (scriptsDir.getCanonicalPath ());
            } catch (IOException e) {
                e.printStackTrace();
            }
            Log.getLog ().info ("Full path to dialogue script files: " + scriptsDir);
            im.setScriptsDir(scriptsDir);            
        }

        if (opts.scriptFile != null) {
            File scriptFile = new File (scriptsDir, opts.scriptFile);
            if (!scriptFile.exists ()) {
                Log.getLog ().warn ("Script file " + scriptFile + " does not exist!");
                return false;
            }
            im.setDialogueScriptFile (opts.scriptFile);
        }

        File scenarioDir = null;
        if (opts.scenarioDir != null) {
        	scenarioDir = new File (opts.scriptsDir);
        }
        if (scenarioDir == null || !scenarioDir.exists ()) {
            Log.getLog ().warn ("Scenarios directory " + opts.scenarioDir + " does not exist!");
            return false;
        } else {
            try {
            	scenarioDir = new File (scenarioDir.getCanonicalPath ());
            } catch (IOException e) {
                e.printStackTrace();
            }
            Log.getLog ().info ("Full path to scenario files: " + scenarioDir);
            //im.setScenariosDir(scenarioDir);            
        }
        
        return true;
    }

}
