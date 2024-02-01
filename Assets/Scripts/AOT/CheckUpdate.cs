using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CheckUpdate
{
    Action doneCallBack;

    public CheckUpdate(Action call)
    {
        doneCallBack = call;
    }
    public void Check()
    {
        //读取服务器版本信息
        DownloadHandler(LoadData.SERVER_VERSION_PATH, CheckNeedUpdate);
    }

    public void CheckNeedUpdate(byte[] result)
    {
        string SverJson = Encoding.GetEncoding("utf-8").GetString(result);
        VersionData SverData = JsonUtility.FromJson<VersionData>(SverJson);
        if(SverData.pkgVersion != LoadData.PACKAGE_VERSION)
        {
            if(SverData.forceUpdate == 1)
            {
                Debug.Log("当前有新的版本需要更新，否则无法继续游戏！");
                return;
            }
            Debug.Log("当前有新的版本可更新");
        }
        Debug.Log("当前包体为最新版本");
        doneCallBack.Invoke();
    }

    public void DownloadHandler(string url, Action<byte[]> callback)
    {
        download(url, callback);
    }

    async void download(string url, Action<byte[]> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log($"下载{url}时发生错误{request.error}");
            return;
        }
        while (request.result != UnityWebRequest.Result.Success)
        {
            //下载过程中
            await UniTask.NextFrame();
        }
        if (request.isDone)
        {
            Debug.Log($"下载{url}完毕,字节：" + request.downloadHandler.data.Length);
            callback(request.downloadHandler.data);
        }
    }
}
