using System;
using System.Runtime.CompilerServices;

namespace OmokClient.Packets
{
    public class DevLog
    {
        static System.Collections.Concurrent.ConcurrentQueue<string> logMsgQueue = new System.Collections.Concurrent.ConcurrentQueue<string>();
        static public void Write(string msg,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string methodName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            logMsgQueue.Enqueue(string.Format("{0}:{1}| {2}", DateTime.Now, methodName, msg));
        }
        static public bool GetLog(out string msg)
        {
            if (logMsgQueue.TryDequeue(out msg))
            {
                return true;
            }

            return false;
        }
        
    }
}