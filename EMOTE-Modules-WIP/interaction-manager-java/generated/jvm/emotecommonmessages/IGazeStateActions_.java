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
public final class IGazeStateActions_ {
    
    //<generated-static>
    private static system.Type staticType;
    
    public static system.Type typeof() {
        return emotecommonmessages.IGazeStateActions_.staticType;
    }
    
    private static void InitJNI(net.sf.jni4net.inj.INJEnv env, system.Type staticType) {
        emotecommonmessages.IGazeStateActions_.staticType = staticType;
    }
    //</generated-static>
}

//<generated-proxy>
@net.sf.jni4net.attributes.ClrProxy
class __IGazeStateActions extends system.Object implements emotecommonmessages.IGazeStateActions {
    
    protected __IGazeStateActions(net.sf.jni4net.inj.INJEnv __env, long __handle) {
            super(__env, __handle);
    }
    
    @net.sf.jni4net.attributes.ClrMethod("(DD)V")
    public native void GazeAtScreen(double x, double y);
    
    @net.sf.jni4net.attributes.ClrMethod("(DD)V")
    public native void GlanceAtScreen(double x, double y);
    
    @net.sf.jni4net.attributes.ClrMethod("(LSystem/String;)V")
    public native void GazeAtTarget(java.lang.String targetName);
    
    @net.sf.jni4net.attributes.ClrMethod("(LSystem/String;)V")
    public native void GlanceAtTarget(java.lang.String targetName);
}
//</generated-proxy>
