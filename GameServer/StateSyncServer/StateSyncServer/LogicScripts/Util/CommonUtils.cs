using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Util
{
    public class CommonUtils
    {

        public static string GetCurTime()
        {
            return System.DateTime.Now.ToString("HH:mm:ss:fff");
        }

        public static void Logout(string content)
        {
            Console.WriteLine($"[{GetCurTime()}]{content}");
        }
    }
}
