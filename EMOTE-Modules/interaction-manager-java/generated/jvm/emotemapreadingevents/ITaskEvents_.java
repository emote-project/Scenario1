// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by jni4net. See http://jni4net.sourceforge.net/ 
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

package emotemapreadingevents;

import redstone.xmlrpc.XmlRpcArray;

@net.sf.jni4net.attributes.ClrTypeInfo
public final class ITaskEvents_ {
    
    //<generated-static>
    private static system.Type staticType;
    
    public static system.Type typeof() {
        return emotemapreadingevents.ITaskEvents_.staticType;
    }
    
    private static void InitJNI(net.sf.jni4net.inj.INJEnv env, system.Type staticType) {
        emotemapreadingevents.ITaskEvents_.staticType = staticType;
    }
    //</generated-static>
}

//<generated-proxy>
@net.sf.jni4net.attributes.ClrProxy
class __ITaskEvents extends system.Object implements emotemapreadingevents.ITaskEvents {
    
    protected __ITaskEvents(net.sf.jni4net.inj.INJEnv __env, long __handle) {
            super(__env, __handle);
    }
    
    @net.sf.jni4net.attributes.ClrMethod("(IIIIIZ[LSystem/String;[Z[LSystem/String;[LSystem/String;LSystem/String;)V")
    public native void stepAnswerAttempt(int learnerId, String learnerName, int stepId, int activityId, int scenarioId, int sessionId, boolean correct, java.lang.String[] competencyName, boolean[] competencyCorrect, java.lang.String[] competencyActual, java.lang.String[] competencyExpected, java.lang.String mapEventId);
    
    @net.sf.jni4net.attributes.ClrMethod("(IIIIILSystem/String;LSystem/String;LSystem/String;)V")
    public native void interactionEvaluation(int learnerId, int stepId, int activityId, int scenarioId, int sessionId, java.lang.String action, java.lang.String strategy, java.lang.String evaluation);

	@Override
	public void stepAnswerAttempt(int learnerId, String learnerName,
			int stepId, int activityId, int scenarioId, int sessionId,
			boolean correct, XmlRpcArray competencyName,
			XmlRpcArray competencyCorrect, XmlRpcArray competencyActual,
			XmlRpcArray competencyExpected, XmlRpcArray competencySkillLevels, String mapEventId) {
		// TODO Auto-generated method stub
		
	}

	
		
	
}
//</generated-proxy>
