using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Util
{
    public static class CommonUtils
    {

        public static string GetCurTime()
        {
            return System.DateTime.Now.ToString("HH:mm:ss:fff");
        }

        public static void Logout(string content)
        {
            Console.WriteLine($"[{GetCurTime()}]{content}");
        }

        public static Vector3 Normalize(this Vector3 v)
        {
            float len = v.Length();
            Vector3 result = new Vector3(v.X, v.Y, v.Z) / len;
            return result;
        }
    }
}
