// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by jni4net. See http://jni4net.sourceforge.net/ 
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

package emoteenercitiesmessages;

import redstone.xmlrpc.XmlRpcArray;

@net.sf.jni4net.attributes.ClrInterface
public interface IEnercitiesAIActions {
    
    //<generated-interface>
    @net.sf.jni4net.attributes.ClrMethod("(LSystem/String;)V")
    void StrategiesUpdated(java.lang.String StrategiesSet_strategies);
    
    @net.sf.jni4net.attributes.ClrMethod("([LSystem/String;)V")
    void BestActionPlanned(java.lang.String[] EnercitiesActionInfo_actionInfos);
    
    @net.sf.jni4net.attributes.ClrMethod("([LSystem/String;)V")
    // void BestActionsPlanned(java.lang.String[] EnercitiesActionInfo_actionInfos);
    void BestActionsPlanned(XmlRpcArray EnercitiesActionInfo_actionInfos);
    
    @net.sf.jni4net.attributes.ClrMethod("([D)V")
    void PredictedValuesUpdated(double[] values);
    //</generated-interface>
}