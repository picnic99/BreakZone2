using SimpleJSON;

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