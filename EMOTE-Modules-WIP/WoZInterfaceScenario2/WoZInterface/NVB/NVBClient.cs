using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;

namespace WOZInterface.NVB
{

   

    class NVBClient
    {
        NonVerbalBehavior window;

        public NVBClient(NonVerbalBehavior window)
        {
            this.window = window;
            WoZOptimusPrimeMegaClient.GetInstance().TargetScreenInfoEvent += NVBClient_TargetScreenInfoEvent;
        }

        void NVBClient_TargetScreenInfoEvent(object sender, WoZOptimusPrimeMegaClient.TargetsEventArgs e)
        {
            window.AddTarget(e.TargetName);
        }
    
    }

}
