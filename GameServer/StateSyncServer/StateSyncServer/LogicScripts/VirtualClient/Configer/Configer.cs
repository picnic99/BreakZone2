using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using System.Text.Json;

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

        private static JsonElement LoadJson(string file)
        {
            var temp = ResourceManager.GetInstance().GetConfigRes(file);
            JsonElement jsonElement = JsonSerializer.Deserialize<JsonElement>(temp);
            return jsonElement;
        }
    }
}