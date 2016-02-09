package uk.ac.hw.emote.intman.listener;

import uk.ac.hw.emote.intman.dm.TurnTakingManager;
import emotecommonmessages.IFMLSpeechEvents;

public class SkeneListener implements IFMLSpeechEvents {

    @Override
    public void UtteranceStarted (String id) {
    	if (!id.equals("")){
    		TurnTakingManager.getInstance ().utteranceStarted (id);
    	}
    }

    @Override
    public void UtteranceFinished (String id) {
    	if (!id.equals("")){
    		TurnTakingManager.getInstance ().utteranceFinished (id);
    	}
    }

}
