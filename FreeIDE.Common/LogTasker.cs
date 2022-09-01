using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace FreeIDE.Common
{
    public class LogTasker
    {
        public static Queue<Action> ActionTrain = new Queue<Action>();
        public static Timer ActionsInvokeTimer;

        public static void Init()
        {
            ActionsInvokeTimer = new Timer(1000); // add to settings
            ActionsInvokeTimer.Elapsed += ActionsInvokeTimer_Elapsed;
            ActionsInvokeTimer.Start();
        }

        public static void AddInvoke(Action action)
        {
            ActionTrain.Enqueue(action);
        }

        public static void ForceCompleteTasks()
        {
            CompleteTasks();
        }

        private static void ActionsInvokeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CompleteTasks();
        }

        private static void CompleteTasks()
        {
            if (ActionTrain.Count <= 0) return;

            ActionsInvokeTimer.Stop();

            for (int i = 0; i < ActionTrain.Count; i++)
                Task.Run(ActionTrain.Dequeue()).Wait();

            ActionsInvokeTimer.Start();
        }
    }
}
