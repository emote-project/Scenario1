// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by jni4net. See http://jni4net.sourceforge.net/ 
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

package emotecommonmessages;

@net.sf.jni4net.attributes.ClrTypeInfo
public final class IWavingStateAction_ {
    
    //<generated-static>
    private static system.Type staticType;
    
    public static system.Type typeof() {
        return emotecommonmessages.IWavingStateAction_.staticType;
    }
    
    private static void InitJNI(net.sf.jni4net.inj.INJEnv env, system.Type staticType) {
        emotecommonmessages.IWavingStateAction_.staticType = staticType;
    }
    //</generated-static>
}

//<generated-proxy>
@net.sf.jni4net.attributes.ClrProxy
class __IWavingStateAction extends system.Object implements emotecommonmessages.IWavingStateAction {
    
    protected __IWavingStateAction(net.sf.jni4net.inj.INJEnv __env, long __handle) {
            super(__env, __handle);
    }
    
    @net.sf.jni4net.attributes.ClrMethod("(LSystem/String;)V")
    public native void WaveAtTarget(java.lang.String targetName);
    
    @net.sf.jni4net.attributes.ClrMethod("(LSystem/String;DDDDD)V")
    public native void WaveAtScreen(java.lang.String id, double x, double y, double amplitude, double frequency, double duration);
}
//</generated-proxy>
