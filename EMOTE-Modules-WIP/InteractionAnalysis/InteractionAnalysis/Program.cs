using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace IA
{
    public class Program
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);      
        
        static Thread[] mThreads = new Thread[4];
   
        static void Main(string[] args)
        {
            string mDataFolder = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "InteractionAnalysis\\lib");
            SetDllDirectory(mDataFolder);
            
            InteractionAnalysis  mInteractionAnalysis = new InteractionAnalysis();

            mThreads[0] = new Thread(new ThreadStart(mInteractionAnalysis.InitThalamusClient));
            mThreads[0].Start();

            mThreads[1] = new Thread(new ThreadStart(mInteractionAnalysis.SetupPipeConnection));
            mThreads[2] = new Thread(new ThreadStart(mInteractionAnalysis.ProcessSegment));
            mThreads[3] = new Thread(new ThreadStart(mInteractionAnalysis.KeyListener));
            
            mThreads[1].Start();
            mThreads[2].Start();
            mThreads[3].Start();

            WaitHandle.WaitAll(mInteractionAnalysis.mEvents);         
            mInteractionAnalysis.Shutdown();       
        }       
    }
}