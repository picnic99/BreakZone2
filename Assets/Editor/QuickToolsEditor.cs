
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


    [MenuItem("Quick/打开PB文件夹", priority = 99)]
    public static void UpdatePBForCS()
    {
        var dir = "PBtools\\proto";
        ShellExecute(IntPtr.Zero, "open", RootURL() + dir, "", "", 1);
    }

    [MenuItem("Quick/编译PB", priority = 99)]
    public static void ComplierPB()
    {
        var dir = "PBtools\\proto2C#.bat";
        ShellExecute(IntPtr.Zero, "open", RootURL() + dir, "", "", 1);
    }

    [MenuItem("Quick/更新双端PB", priority = 99)]
    public static void CopyPB()
    {
        File.Copy(RootURL() + "PBtools\\output\\GamePbProtocol.cs", RootURL() + "Assets\\LogicScripts\\Client\\Net\\PB\\GamePbProtocol.cs", true);
        File.Copy(RootURL() + "PBtools\\output\\GamePbProtocol.cs", RootURL() + "GameServer\\StateSyncServer\\StateSyncServer\\LogicScripts\\Net\\PB\\GamePbProtocol.cs", true);
        Debug.Log("GamePbProtocol文件拷贝完成!");
    }

    [MenuItem("Quick/打开服务器目录", priority = 99)]
    public static void OpenServer()
    {
        var dir = "GameServer\\StateSyncServer\\StateSyncServer";
        ShellExecute(IntPtr.Zero, "open", RootURL() + dir, "", "", 1);
    }

    [MenuItem("Quick/同步PB协议到服务端", priority = 99)]
    public static void SyncPBtoCS()
    {
        string from = RootURL() + "Assets\\LogicScripts\\Client\\Net\\PB";
        string to = RootURL() + "GameServer\\StateSyncServer\\StateSyncServer\\LogicScripts\\Net\\PB";
        CopyDirectory(from, to);
    }

    [MenuItem("Quick/启动资源服务器", priority = 99)]
    public static void LuncherResServer()
    {
        var dir = "GameServer\\ResServer\\hfs.exe";
        ShellExecute(IntPtr.Zero, "open", RootURL() + dir, "", "", 1);
    }

    [MenuItem("Quick/更新热更代码", priority = 101)]
    public static void UpdateHotCode()
    {
        CompileDllCommand.CompileDllActiveBuildTarget();
    }

    [MenuItem("Quick/执行完整热更流程", priority = 102)]
    public static void UpdateTotalCode()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        CompileDllCommand.CompileDll(target);
        Il2CppDefGeneratorCommand.GenerateIl2CppDef();

        // 这几个生成依赖HotUpdateDlls
        LinkGeneratorCommand.GenerateLinkXml(target);

        // 生成裁剪后的aot dll
        StripAOTDllCommand.GenerateStripedAOTDlls(target);

        // 桥接函数生成依赖于AOT dll，必须保证已经build过，生成AOT dll
        MethodBridgeGeneratorCommand.GenerateMethodBridge(target);
        ReversePInvokeWrapperGeneratorCommand.GenerateReversePInvokeWrapper(target);
        AOTReferenceGeneratorCommand.GenerateAOTGenericReference(target);

        //输出热更DLL
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
        // 获取源文件夹的信息  
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        // 如果目标目录不存在，则创建它  
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // 获取源文件夹中的文件列表  
        FileInfo[] files = dir.GetFiles();

        // 复制文件  
        foreach (FileInfo file in files)
        {
            if (file.Extension == ".meta") continue;
            string tempPath = Path.Combine(destDirName, file.Name);
            file.CopyTo(tempPath, true); // 第二个参数为true表示覆盖同名文件  
        }

        // 获取源文件夹中的子文件夹列表  
        DirectoryInfo[] dirSubDirs = dir.GetDirectories();

        // 递归复制子文件夹  
        foreach (DirectoryInfo subDir in dirSubDirs)
        {
            string tempPath = Path.Combine(destDirName, subDir.Name);
            CopyDirectory(subDir.FullName, tempPath);
        }
    }
}
