To run the Birmignham WoZ code.

Ensure you have latest thalamus from the seperate repo.
Get the scenario 1 from Aidan's repo. 
Build:
Thalamus
EmoteEvents
SpeakerRapport	
Skene
MapInterface
WoZInterface

Fotis what are the 

Deploy the relevant code to the robot. This is NAO Bridges. 
Ensure the robot has these behaviours...


To run (start everything as admin).
Start: 
thalamus standalone
Skene
MapInterface
WoZInterface
Start stuff running on robot. How do you do that? 

Start the Java server, instructions are in the readme in Aidan's scenario 1 repo.

Start controlling via the Woz interface, add a learner name and id, hit start... 


Some notes on thalamus:
ThalamusStandalone is used to create and hold Characters.
All the modules connect to one Character (which has a name when you create it)
If you launch a client module with a command line parameter, that means that the client will only connect to a Character with the same name as the parameter.
Check the debug properties of the VS project to see if its adding a character name.
Change it to your character's name.
This is used because Thalamus works across the local network - and this way several people can be working with thalamus at the same time.

For example, I create a character called Tiago, eugenio creates one callex Eux, so I tell my modules to connect to the Tiago character.
If you don't specify which character it should connect to, the client connects to the first one if gets, and that sometimes leads to for ex, me launching a client but I don't see it in my Thalamus, and suddenly Eugenio says "hey you have a client connected to my Thalamus!"

Second, you can also launch ThalamusStandalone with parameters.
If it is just a word, then it will automatically create a character using that word as the name. It's really usefull so you don't always have to go clicking around everythime you launch things.

After you have several clients connected to a character, you can also save that as a Scenario, it's in the bottom left of the ThalamusStandalow window.
Next time you just double-click that scenario and all clients will automatically be launched.
You can also have the Standalone load a scenario when launched, just add "-s" to the command-line arguments, and instead of creating a character with the specified name, it will attempt to load the scenario with the specified name.


Some examples to explain:
(etc..)\ThalamusStandalone.exe -> Launches thalamus and does nothing else
(etc..)\ThalamusStandalone.exe Tiago -> Launches thalamus and automatically creates a character called Tiago
(etc..)\Skene.exe -> Launches Skene and connects to the first Thalamus character it gets (if you are working alone in the network then this works fine)
(etc..)\Skene.exe Tiago -> Launches Skene but only connects it to a Thalamus character named Tiago (highly recommended if you are sharing the network with other people using thalamus)

Assuming you have saved a scenario called EmoteWoz1:
(etc..)\ThalamusStandalone.exe EmoteWoz1 -s -> Launches Thalamus and load the scenario called EmoteWoz1, which automatically launches all the clients you saved in it.

CSV Logging:
(etc..)\ThalamusStandalone.exe EmoteWoz1 -s -csv -> Does the same as above but activates the Log do CSV checkbox, which you SHOULD use in order to save all the messages to separate CSV files, which are more appropriate to load, say, into Elan.
CSV logging is disabled by default because it creates a huge amount of log files, but it should be used in studies.
This is a recent feature, as far as I know, there hasn't been any study yet using this, but now is a good time to tell you about it.


