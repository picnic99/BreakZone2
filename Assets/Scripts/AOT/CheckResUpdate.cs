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
        // ��ʼ����Դϵͳ
        YooAssets.Initialize();

        // ����Ĭ�ϵ���Դ��
        package = YooAssets.CreatePackage("DefaultPackage");

        // ���ø���Դ��ΪĬ�ϵ���Դ��������ʹ��YooAssets��ؼ��ؽӿڼ��ظ���Դ�����ݡ�
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

        //��Դ����ʼ��
        initOperation = package.InitializeAsync(initParameters);
        await initOperation;

        if (initOperation.Status == EOperationStatus.Succeed)
        {
            Debug.Log("��Դ����ʼ���ɹ���");
            UpdatePackageVersion();
        }
        else
        {
            Debug.LogError($"��Դ����ʼ��ʧ�ܣ�{initOperation.Error}");
        }
    }


    string packageVersion;
    /// <summary>
    /// ���²����嵥
    /// </summary>
    /// <returns></returns>
    async void UpdatePackageVersion()
    {
        var package = YooAssets.GetPackage("DefaultPackage");
        var operation = package.UpdatePackageVersionAsync(false);
        await operation;

        if (operation.Status == EOperationStatus.Succeed)
        {
            //���³ɹ�
            packageVersion = operation.PackageVersion;
            Debug.Log($"���²����嵥 �汾Ϊ: {packageVersion}");
            UpdatePackageManifest();
        }
        else
        {
            //����ʧ��
            Debug.LogError(operation.Error);
        }
    }

    async void UpdatePackageManifest()
    {
        // ���³ɹ����Զ�����汾�ţ���Ϊ�´γ�ʼ���İ汾��
        // Ҳ����ͨ��operation.SavePackageVersion()�������档
        bool savePackageVersion = true;
        var package = YooAssets.GetPackage("DefaultPackage");
        var operation = package.UpdatePackageManifestAsync(packageVersion, savePackageVersion);
        await operation;

        if (operation.Status == EOperationStatus.Succeed)
        {
            //���³ɹ�
            Debug.Log($"��Դ�����³ɹ�");
            Download();
        }
        else
        {
            //����ʧ��
            Debug.LogError(operation.Error);
        }
    }

    async void Download()
    {
        int downloadingMaxNum = 10;
        int failedTryAgain = 3;
        var package = YooAssets.GetPackage("DefaultPackage");
        var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

        //û����Ҫ���ص���Դ
        if (downloader.TotalDownloadCount == 0)
        {
            doneCallBack.Invoke();
            return;
        }

        //��Ҫ���ص��ļ��������ܴ�С
        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadBytes;

        //ע��ص�����
/*        downloader.OnDownloadErrorCallback = OnDownloadErrorFunction;
        downloader.OnDownloadProgressCallback = OnDownloadProgressUpdateFunction;
        downloader.OnDownloadOverCallback = OnDownloadOverFunction;
        downloader.OnStartDownloadFileCallback = OnStartDownloadFileFunction;*/

        //��������
        downloader.BeginDownload();
        await downloader;

        //������ؽ��
        if (downloader.Status == EOperationStatus.Succeed)
        {
            //���سɹ�
            doneCallBack.Invoke();
        }
        else
        {
            //����ʧ��
            Debug.Log($"��Դ������ʧ��");
        }
    }
}
