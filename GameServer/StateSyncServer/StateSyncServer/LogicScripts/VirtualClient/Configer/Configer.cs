using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Configer
{
    /// <summary>
    /// 配置加载类
    /// 负责加载LUBAN配置表数据信息
    /// </summary>
    public class Configer
    {
        private static cfg.Tables tables;
        public static cfg.Tables Tables
        {
            get
            {
                if (tables == null)
                {
                    Load();
                }
                return tables;
            }
        }

        static void Load()
        {
            tables = new cfg.Tables(LoadJson);
        }

        private static JSONNode LoadJson(string file)
        {
            var temp = ResourceManager.GetInstance().GetConfigRes(file);
            return JSON.Parse(temp.text);
        }
    }
}
