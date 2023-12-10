using SimpleJSON;
using System.IO;
using UnityEngine;

/// <summary>
/// 配置加载类
/// 负责加载LUBAN配置表数据信息
/// </summary>
public class Configer
{
    private static cfg.Tables tables;
    public static cfg.Tables Tables { 
        get {
            if (tables==null)
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
        return JSON.Parse(File.ReadAllText(Application.dataPath + $"/../Config/output/json/{file}.json", System.Text.Encoding.UTF8));
    }
}