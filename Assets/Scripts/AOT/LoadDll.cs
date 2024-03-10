using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using YooAsset;

/// <summary>
/// 热更代码模块
/// </summary>
public class LoadDll : MonoBehaviour
{
    void Start()
    {
        Debug.Log("热更新流程开始");
#if UNITY_EDITOR
        CheckRes();
#else
        CheckPkg();
#endif
    }

    public void CheckPkg()
    {
        //包体检查 判断是否需要强更包体
        var check = new CheckUpdate(CheckRes);
        check.Check();
    }

    /// <summary>
    /// 资源检查
    /// </summary>
    public void CheckRes()
    {
        var check = new CheckResUpdate(StartLoad);
        check.Check();
    }

    /// <summary>
    /// 加载热更代码 进入游戏页面
    /// </summary>
    public void StartLoad()
    {
        var package = YooAssets.GetPackage("DefaultPackage");

#if !UNITY_EDITOR

        //补充元数据
        List<string> aotDllList = new List<string>
        {
            "mscorlib.dll.bytes",
            "System.dll.bytes",
            "System.Core.dll.bytes", // 如果使用了Linq，需要这个
            // "Newtonsoft.Json.dll",
            // "protobuf-net.dll",
            "UnityEngine.CoreModule.dll.bytes",
            "Google.Protobuf.dll.bytes"
        };

        for (int i = 0; i < aotDllList.Count; i++)
        {
            string aotDllName = aotDllList[i];
            AssetHandle temp = package.LoadAssetSync("Assets/Res/dll/" + aotDllName);
            byte[] dll = temp.GetAssetObject<TextAsset>().bytes;

            LoadImageErrorCode err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dll, HomologousImageMode.SuperSet);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
        }

        //加载热更代码
        AssetHandle handlefiles = package.LoadAssetSync("Assets/Res/code/HotUpdate.dll.bytes");
        byte[] dllBytes = handlefiles.GetAssetObject<TextAsset>().bytes;
        Assembly hotUpdateAss = Assembly.Load(dllBytes);
#else
        // Editor下无需加载，直接查找获得HotUpdate程序集
        Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == LoadData.HOT_CODE);
#endif

        //进入游戏流程
        SceneHandle sceneData = package.LoadSceneAsync("Assets/Res/Scene/Root");
        sceneData.Completed += LoadScene;
    }

    public void LoadScene(SceneHandle data)
    {
        SceneManager.SetActiveScene(data.SceneObject);
    }

}
