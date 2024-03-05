using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogicScripts.Utils
{
    class CommonUtils
    {
        public static string GetCurTime()
        {
            return System.DateTime.Now.ToString("HH:mm:ss:fff");
        }

        public static void Logout(string content)
        {
            Debug.Log($"[{GetCurTime()}]{content}");
        }
    }
}
