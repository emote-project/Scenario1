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
public final class IEnercitiesTaskEvents_ {
    
    //<generated-static>
    private static system.Type staticType;
    
    public static system.Type typeof() {
        return emoteenercitiesmessages.IEnercitiesTaskEvents_.staticType;
    }
    
    private static void InitJNI(net.sf.jni4net.inj.INJEnv env, system.Type staticType) {
        emoteenercitiesmessages.IEnercitiesTaskEvents_.staticType = staticType;
    }
    //</generated-static>
}

//<generated-proxy>
@net.sf.jni4net.attributes.ClrProxy
class __IEnercitiesTaskEvents extends system.Object implements emoteenercitiesmessages.IEnercitiesTaskEvents {
    
    protected __IEnercitiesTaskEvents(net.sf.jni4net.inj.INJEnv __env, long __handle) {
            super(__env, __handle);
    }
    
    @net.sf.jni4net.attributes.ClrMethod("(LEmoteEnercitiesMessages/StructureType;LSystem/String;II)V")
    public native void ConfirmConstruction(emoteenercitiesmessages.StructureType structure, java.lang.String translation, int cellX, int cellY);
    
    @net.sf.jni4net.attributes.ClrMethod("(LEmoteEnercitiesMessages/PolicyType;LSystem/String;)V")
    public native void ImplementPolicy(emoteenercitiesmessages.PolicyType policy, java.lang.String translation);
    
    @net.sf.jni4net.attributes.ClrMethod("(LEmoteEnercitiesMessages/UpgradeType;LSystem/String;II)V")
    public native void PerformUpgrade(emoteenercitiesmessages.UpgradeType upgrade, java.lang.String translation, int cellX, int cellY);
    
    @net.sf.jni4net.attributes.ClrMethod("()V")
    public native void SkipTurn();
    
    @net.sf.jni4net.attributes.ClrMethod("(DDIILEmoteEnercitiesMessages/StructureType;LSystem/String;)V")
    public native void ExamineCell(double screenX, double screenY, int cellX, int cellY, emoteenercitiesmessages.StructureType StructureType_structure, java.lang.String StructureType_translated);

	@Override
	public void ConfirmConstruction(String structure, String translation,
			int cellX, int cellY) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void ImplementPolicy(String policy, String translation) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void PerformUpgrade(String upgrade, String translation, int cellX,
			int cellY) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void ExamineCell(double screenX, double screenY, int cellX,
			int cellY, String StructureType_structure,
			String StructureType_translated) {
		// TODO Auto-generated method stub
		
	}
}
//</generated-proxy>