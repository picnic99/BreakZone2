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
/// �ȸ�����ģ��
/// </summary>
public class LoadDll : MonoBehaviour
{
    void Start()
    {
        Debug.Log("�ȸ������̿�ʼ");
#if UNITY_EDITOR
        CheckRes();
#else
        CheckPkg();
#endif
    }

    public void CheckPkg()
    {
        //������ �ж��Ƿ���Ҫǿ������
        var check = new CheckUpdate(CheckRes);
        check.Check();
    }

    /// <summary>
    /// ��Դ���
    /// </summary>
    public void CheckRes()
    {
        var check = new CheckResUpdate(StartLoad);
        check.Check();
    }

    /// <summary>
    /// �����ȸ����� ������Ϸҳ��
    /// </summary>
    public void StartLoad()
    {
        var package = YooAssets.GetPackage("DefaultPackage");

#if !UNITY_EDITOR

        //����Ԫ����
        List<string> aotDllList = new List<string>
        {
            "mscorlib.dll.bytes",
            "System.dll.bytes",
            "System.Core.dll.bytes", // ���ʹ����Linq����Ҫ���
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

        //�����ȸ�����
        AssetHandle handlefiles = package.LoadAssetSync("Assets/Res/code/HotUpdate.dll.bytes");
        byte[] dllBytes = handlefiles.GetAssetObject<TextAsset>().bytes;
        Assembly hotUpdateAss = Assembly.Load(dllBytes);
#else
        // Editor��������أ�ֱ�Ӳ��һ��HotUpdate����
        Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == LoadData.HOT_CODE);
#endif

        //������Ϸ����
        SceneHandle sceneData = package.LoadSceneAsync("Assets/Res/Scene/Root");
        sceneData.Completed += LoadScene;
    }

    public void LoadScene(SceneHandle data)
    {
        SceneManager.SetActiveScene(data.SceneObject);
    }

}
