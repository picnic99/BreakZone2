using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using YooAsset;

public class LoadData
{
    //当前包体版本
    public static string PACKAGE_VERSION = "1.0";

    // 资源系统运行模式
# if UNITY_EDITOR
    public static EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
#else
    public static EPlayMode PlayMode = EPlayMode.HostPlayMode;
#endif
    //资源服务器地址
    private static string ServerUrl => GetServerURL();

    //补丁路径
    private static string patchPath = "BZ/CDN/" + GetPlatformPath() + "/v1.0";

    //包体路径
    private static string packPath = "BZ/package/" + GetPlatformPath() + "/v1.0";

    //本地版本信息
    public static string localVerPath = Application.streamingAssetsPath + "/version.json";

    //服务器资源完整路径
    public static string RES_SERVER_PATH = ServerUrl + patchPath;

    //服务器包体完整路径
    public static string PACKAGE_SERVER_PATH = ServerUrl + "BZ/" + packPath;

    //服务器版本信息路径
    public static string SERVER_VERSION_PATH = ServerUrl + "BZ/version.json";

    //热更代码名称
    public static string HOT_CODE = "HotUpdate";

    public static string GetPlatformPath()
    {
        string path = "";
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                path = "ios";
                break;
            case RuntimePlatform.WindowsPlayer:
                path = "pc";
                break;
            case RuntimePlatform.Android:
                path = "android";
                break;
            default:
                break;
        }
        return path;
    }

    /// <summary>
    /// 获取服务器地址 走外围配置
    /// 后续 需要改到公网服务器
    /// 从服务器
    /// </summary>
    /// <returns></returns>
    public static string GetServerURL()
    {
        string url = File.ReadAllText(Application.streamingAssetsPath+"/url.txt",System.Text.Encoding.UTF8);
        return url;
    }
}
