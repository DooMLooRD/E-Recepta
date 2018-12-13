using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain
{
    public class TraceingManager
    {
        public static void InitializeTraceing()
        {
            string path = Directory.GetCurrentDirectory() + "\\TraceInfo.txt";
            Trace.Listeners.Add(new TextWriterTraceListener( path, "BlockChainTraceingListener"));
        }
        public static void Message(string message)
        {
            Trace.TraceInformation(DateTime.Now.ToString() + " :: " + message);
            Trace.Flush();
        }
    }
}
