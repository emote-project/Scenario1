// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by jni4net. See http://jni4net.sourceforge.net/ 
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

package emoteenercitiesmessages;

@net.sf.jni4net.attributes.ClrTypeInfo
public final class IEnercitiesExamineEvents_ {
    
    //<generated-static>
    private static system.Type staticType;
    
    public static system.Type typeof() {
        return emoteenercitiesmessages.IEnercitiesExamineEvents_.staticType;
    }
    
    private static void InitJNI(net.sf.jni4net.inj.INJEnv env, system.Type staticType) {
        emoteenercitiesmessages.IEnercitiesExamineEvents_.staticType = staticType;
    }
    //</generated-static>
}

//<generated-proxy>
@net.sf.jni4net.attributes.ClrProxy
class __IEnercitiesExamineEvents extends system.Object implements emoteenercitiesmessages.IEnercitiesExamineEvents {
    
    protected __IEnercitiesExamineEvents(net.sf.jni4net.inj.INJEnv __env, long __handle) {
            super(__env, __handle);
    }
    
    @net.sf.jni4net.attributes.ClrMethod("(LEmoteEnercitiesMessages/StructureCategory;LSystem/String;)V")
    public native void BuildMenuTooltipShowed(emoteenercitiesmessages.StructureCategory category, java.lang.String translation);
    
    @net.sf.jni4net.attributes.ClrMethod("(LEmoteEnercitiesMessages/StructureCategory;LSystem/String;)V")
    public native void BuildMenuTooltipClosed(emoteenercitiesmessages.StructureCategory category, java.lang.String translation);
    
    @net.sf.jni4net.attributes.ClrMethod("(LEmoteEnercitiesMessages/StructureType;LSystem/String;)V")
    public native void BuildingMenuToolSelected(emoteenercitiesmessages.StructureType structure, java.lang.String translation);
    
    @net.sf.jni4net.attributes.ClrMethod("(LEmoteEnercitiesMessages/StructureType;LSystem/String;)V")
    public native void BuildingMenuToolUnselected(emoteenercitiesmessages.StructureType structure, java.lang.String translation);
    
    @net.sf.jni4net.attributes.ClrMethod("()V")
    public native void PoliciesMenuShowed();
    
    @net.sf.jni4net.attributes.ClrMethod("()V")
    public native void PoliciesMenuClosed();
    
    @net.sf.jni4net.attributes.ClrMethod("(LEmoteEnercitiesMessages/PolicyType;LSystem/String;)V")
    public native void PolicyTooltipShowed(emoteenercitiesmessages.PolicyType policy, java.lang.String translation);
    
    @net.sf.jni4net.attributes.ClrMethod("()V")
    public native void PolicyTooltipClosed();
    
    @net.sf.jni4net.attributes.ClrMethod("()V")
    public native void UpgradesMenuShowed();
    
    @net.sf.jni4net.attributes.ClrMethod("()V")
    public native void UpgradesMenuClosed();
    
    @net.sf.jni4net.attributes.ClrMethod("(LEmoteEnercitiesMessages/UpgradeType;LSystem/String;)V")
    public native void UpgradeTooltipShowed(emoteenercitiesmessages.UpgradeType upgrade, java.lang.String translation);
    
    @net.sf.jni4net.attributes.ClrMethod("()V")
    public native void UpgradeTooltipClosed();
}
//</generated-proxy>
