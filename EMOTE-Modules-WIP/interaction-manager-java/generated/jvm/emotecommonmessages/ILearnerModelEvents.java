// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by jni4net. See http://jni4net.sourceforge.net/ 
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

package emotecommonmessages;

@net.sf.jni4net.attributes.ClrInterface
public interface ILearnerModelEvents {
    
    //<generated-interface>
    @net.sf.jni4net.attributes.ClrMethod("(IIIIILSystem/String;Z[LSystem/String;[Z[LSystem/String;[LSystem/String;[D[I[D[D[LEmoteCommonMessages/AffectPerceptionState;[LEmoteCommonMessages/Charge;[I)V")
    void learnerModelValueUpdate(
                int learnerId, 
                int stepId, 
                int activityId, 
                int scenarioId, 
                int sessionId, 
                java.lang.String reasonForUpdate, 
                boolean correct, 
                java.lang.String[] competencyName, 
                boolean[] competencyCorrect, 
                java.lang.String[] competencyActual, 
                java.lang.String[] competencyExpected, 
                double[] comptencyValue, 
                int[] competencyConfidence, 
                double[] oldCompetencyValue, 
                double[] competencyDelta, 
                emotecommonmessages.AffectPerceptionState[] state, 
                emotecommonmessages.Charge[] stateCharge, 
                int[] stateConfidence);
    //</generated-interface>
}