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
        //��ȡ�������汾��Ϣ
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
                Debug.Log("��ǰ���µİ汾��Ҫ���£������޷�������Ϸ��");
                return;
            }
            Debug.Log("��ǰ���µİ汾�ɸ���");
        }
        Debug.Log("��ǰ����Ϊ���°汾");
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
            Debug.Log($"����{url}ʱ��������{request.error}");
            return;
        }
        while (request.result != UnityWebRequest.Result.Success)
        {
            //���ع�����
            await UniTask.NextFrame();
        }
        if (request.isDone)
        {
            Debug.Log($"����{url}���,�ֽڣ�" + request.downloadHandler.data.Length);
            callback(request.downloadHandler.data);
        }
    }
}
