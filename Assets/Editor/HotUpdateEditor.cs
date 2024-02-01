using HybridCLR.Editor.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class HotUpdateEditor
{
    [MenuItem("Tool/UpdateHotCode",priority = 101)]
    public static void UpdateHotCode()
    {
        CompileDllCommand.CompileDllActiveBuildTarget();
    }

    [MenuItem("Tool/UpdateAllCode", priority = 102)]
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

    [MenuItem("Tool/UpdatePB", priority = 100)]
    public static void UpdatePB()
    {

    }

    [MenuItem("Tool/Launcher LocalResServer", priority = 98)]
    public static void LocalResServer()
    {

    }
}
