using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace IA
{
    public enum ModuleStatus
    {
        None,
        Ready
    }

    public enum Status
    {
        Debugging,
        OKAO_ONLY,
        Full
    }


    
    public class InternalState
    {
        public ModuleStatus mModuleStatus = ModuleStatus.None;
        public Status mState = Status.OKAO_ONLY;
        public System.Timers.Timer mReadyTimer;
        
        public InternalState()
        {
            mModuleStatus = ModuleStatus.None;
        }

        public void SetupTimer()
        {
            mReadyTimer = new System.Timers.Timer();
            mReadyTimer.Interval = 10000; // 10 secs
            mReadyTimer.Elapsed += new ElapsedEventHandler(ReadyTimerEvent);
            mReadyTimer.Start(); 
        }

        void ReadyTimerEvent(object s, ElapsedEventArgs e)
        {
            mModuleStatus = ModuleStatus.Ready;
            mReadyTimer.Stop();
        }


    }
}
