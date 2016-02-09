using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WOZInterface
{
    /// <summary>
    /// Interaction logic for Competence.xaml
    /// </summary>
    public partial class Competence : UserControl
    {
        WOZManager mWOZManager = null;
        
        public Competence(WOZManager wozmanager)
        {
           mWOZManager = wozmanager;
            
           InitializeComponent();

           
        }

        public void SetTaskGrade(UserResponse responseData)
        {
            DistanceTaskGrade.Value = responseData.mDistanceSkillLevel * 100;
            DirectionTaskGrade.Value = responseData.mDirectionSkillLevel * 100;
            SymbolTaskGrade.Value = responseData.mSymbolSkillLevel * 100;
        }

        public void SetWizardGrade()
        {
            mWOZManager.mLoggingManager.AddLogItem(mWOZManager.mTaskScriptManager.GetActiveStepID(), 0, 0, "", 1, 1, DistanceWizardGrade.Value, DirectionWizardGrade.Value, SymbolWizardGrade.Value, 0, 0, "", "", "");
        }
    

        private void GradingChange(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if(mWOZManager.mTaskState == TaskState.STARTED)
                SetWizardGrade();
        }
    }
}
