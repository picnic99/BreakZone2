
using HybridCLR.Editor.Commands;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;
using UnityEditor;
using UnityEngine;

public static class QuickToolsEditor
{
    public static string ServerRootPath = "";


    [MenuItem("Quick/��PB�ļ���", priority = 99)]
    public static void UpdatePBForCS()
    {
        var dir = "PBtools\\proto";
        ShellExecute(IntPtr.Zero, "open", RootURL() + dir, "", "", 1);
    }

    [MenuItem("Quick/����PB", priority = 99)]
    public static void ComplierPB()
    {
        var dir = "PBtools\\proto2C#.bat";
        ShellExecute(IntPtr.Zero, "open", RootURL() + dir, "", "", 1);
    }

    [MenuItem("Quick/����˫��PB", priority = 99)]
    public static void CopyPB()
    {
        File.Copy(RootURL() + "PBtools\\output\\GamePbProtocol.cs", RootURL() + "Assets\\LogicScripts\\Client\\Net\\PB\\GamePbProtocol.cs", true);
        File.Copy(RootURL() + "PBtools\\output\\GamePbProtocol.cs", RootURL() + "GameServer\\StateSyncServer\\StateSyncServer\\LogicScripts\\Net\\PB\\GamePbProtocol.cs", true);
        Debug.Log("GamePbProtocol�ļ��������!");
    }

    [MenuItem("Quick/�򿪷�����Ŀ¼", priority = 99)]
    public static void OpenServer()
    {
        var dir = "GameServer\\StateSyncServer\\StateSyncServer";
        ShellExecute(IntPtr.Zero, "open", RootURL() + dir, "", "", 1);
    }

    [MenuItem("Quick/ͬ��PBЭ�鵽�����", priority = 99)]
    public static void SyncPBtoCS()
    {
        string from = RootURL() + "Assets\\LogicScripts\\Client\\Net\\PB";
        string to = RootURL() + "GameServer\\StateSyncServer\\StateSyncServer\\LogicScripts\\Net\\PB";
        CopyDirectory(from, to);
    }

    [MenuItem("Quick/������Դ������", priority = 99)]
    public static void LuncherResServer()
    {
        var dir = "GameServer\\ResServer\\hfs.exe";
        ShellExecute(IntPtr.Zero, "open", RootURL() + dir, "", "", 1);
    }

    [MenuItem("Quick/�����ȸ�����", priority = 101)]
    public static void UpdateHotCode()
    {
        CompileDllCommand.CompileDllActiveBuildTarget();
    }

    [MenuItem("Quick/ִ�������ȸ�����", priority = 102)]
    public static void UpdateTotalCode()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        CompileDllCommand.CompileDll(target);
        Il2CppDefGeneratorCommand.GenerateIl2CppDef();

        // �⼸����������HotUpdateDlls
        LinkGeneratorCommand.GenerateLinkXml(target);

        // ���ɲü����aot dll
        StripAOTDllCommand.GenerateStripedAOTDlls(target);

        // �ŽӺ�������������AOT dll�����뱣֤�Ѿ�build��������AOT dll
        MethodBridgeGeneratorCommand.GenerateMethodBridge(target);
        ReversePInvokeWrapperGeneratorCommand.GenerateReversePInvokeWrapper(target);
        AOTReferenceGeneratorCommand.GenerateAOTGenericReference(target);

        //����ȸ�DLL
        CompileDllCommand.CompileDllActiveBuildTarget();
    }


    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern int ShellExecute(IntPtr hwnd, string lpszOp, string lpszFile,
                                    string lpszParams, string lpszDir, int FsShowCmd);


    public static string RootURL()
    {
        string path = Application.dataPath;
        path = path.Substring(0, path.Length - 6);
        return path;
    }

    public static void CopyDirectory(string sourceDirName, string destDirName)
    {
        // ��ȡԴ�ļ��е���Ϣ  
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        // ���Ŀ��Ŀ¼�����ڣ��򴴽���  
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // ��ȡԴ�ļ����е��ļ��б�  
        FileInfo[] files = dir.GetFiles();

        // �����ļ�  
        foreach (FileInfo file in files)
        {
            if (file.Extension == ".meta") continue;
            string tempPath = Path.Combine(destDirName, file.Name);
            file.CopyTo(tempPath, true); // �ڶ�������Ϊtrue��ʾ����ͬ���ļ�  
        }

        // ��ȡԴ�ļ����е����ļ����б�  
        DirectoryInfo[] dirSubDirs = dir.GetDirectories();

        // �ݹ鸴�����ļ���  
        foreach (DirectoryInfo subDir in dirSubDirs)
        {
            string tempPath = Path.Combine(destDirName, subDir.Name);
            CopyDirectory(subDir.FullName, tempPath);
        }
    }
}
