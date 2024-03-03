using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Util
{
    /// <summary>
    /// 单例工具类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : new()
    {
        private static T _instance;

        public static T GetInstance()
        {
            if (_instance == null)
            {
                _instance = new T();
                typeof(T).GetMethod("Init")?.Invoke(_instance, null);
            }
            return _instance;
        }
    }
}
