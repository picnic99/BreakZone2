using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class CheckResUpdate
{
    Action doneCallBack;

    ResourcePackage package;

    public CheckResUpdate(Action call)
    {
        doneCallBack = call;
    }

    public void Check()
    {
        Init();
    }

    public void Init()
    {
        // 初始化资源系统
        YooAssets.Initialize();

        // 创建默认的资源包
        package = YooAssets.CreatePackage("DefaultPackage");

        // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
        YooAssets.SetDefaultPackage(package);

        InitializeYooAsset();
    }

    async void InitializeYooAsset()
    {
        InitializeParameters initParameters = null;
        InitializationOperation initOperation;

        switch (LoadData.PlayMode)
        {
            case EPlayMode.EditorSimulateMode:
                initParameters = new EditorSimulateModeParameters();
                var simulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, "DefaultPackage");
                ((EditorSimulateModeParameters)initParameters).SimulateManifestFilePath = simulateManifestFilePath;
                break;
            case EPlayMode.OfflinePlayMode:
                initParameters = new OfflinePlayModeParameters();
                await package.InitializeAsync(initParameters);
                break;
            case EPlayMode.HostPlayMode:
                string defaultHostServer = LoadData.RES_SERVER_PATH;

                initParameters = new HostPlayModeParameters();
                ((HostPlayModeParameters)initParameters).BuildinQueryServices = new GameQueryServices();
                //initParameters.DecryptionServices = new FileOffsetDecryption();
                ((HostPlayModeParameters)initParameters).RemoteServices = new RemoteServices(defaultHostServer, defaultHostServer);
                break;
            case EPlayMode.WebPlayMode:
                break;
            default:
                break;
        }

        //资源包初始化
        initOperation = package.InitializeAsync(initParameters);
        await initOperation;

        if (initOperation.Status == EOperationStatus.Succeed)
        {
            Debug.Log("资源包初始化成功！");
            UpdatePackageVersion();
        }
        else
        {
            Debug.LogError($"资源包初始化失败：{initOperation.Error}");
        }
    }


    string packageVersion;
    /// <summary>
    /// 更新补丁清单
    /// </summary>
    /// <returns></returns>
    async void UpdatePackageVersion()
    {
        var package = YooAssets.GetPackage("DefaultPackage");
        var operation = package.UpdatePackageVersionAsync(false);
        await operation;

        if (operation.Status == EOperationStatus.Succeed)
        {
            //更新成功
            packageVersion = operation.PackageVersion;
            Debug.Log($"更新补丁清单 版本为: {packageVersion}");
            UpdatePackageManifest();
        }
        else
        {
            //更新失败
            Debug.LogError(operation.Error);
        }
    }

    async void UpdatePackageManifest()
    {
        // 更新成功后自动保存版本号，作为下次初始化的版本。
        // 也可以通过operation.SavePackageVersion()方法保存。
        bool savePackageVersion = true;
        var package = YooAssets.GetPackage("DefaultPackage");
        var operation = package.UpdatePackageManifestAsync(packageVersion, savePackageVersion);
        await operation;

        if (operation.Status == EOperationStatus.Succeed)
        {
            //更新成功
            Debug.Log($"资源包更新成功");
            Download();
        }
        else
        {
            //更新失败
            Debug.LogError(operation.Error);
        }
    }

    async void Download()
    {
        int downloadingMaxNum = 10;
        int failedTryAgain = 3;
        var package = YooAssets.GetPackage("DefaultPackage");
        var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

        //没有需要下载的资源
        if (downloader.TotalDownloadCount == 0)
        {
            doneCallBack.Invoke();
            return;
        }

        //需要下载的文件总数和总大小
        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadBytes;

        //注册回调方法
/*        downloader.OnDownloadErrorCallback = OnDownloadErrorFunction;
        downloader.OnDownloadProgressCallback = OnDownloadProgressUpdateFunction;
        downloader.OnDownloadOverCallback = OnDownloadOverFunction;
        downloader.OnStartDownloadFileCallback = OnStartDownloadFileFunction;*/

        //开启下载
        downloader.BeginDownload();
        await downloader;

        //检测下载结果
        if (downloader.Status == EOperationStatus.Succeed)
        {
            //下载成功
            doneCallBack.Invoke();
        }
        else
        {
            //下载失败
            Debug.Log($"资源包更新失败");
        }
    }
}
