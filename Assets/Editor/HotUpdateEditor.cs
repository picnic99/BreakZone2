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

    [MenuItem("Tool/UpdatePB", priority = 100)]
    public static void UpdatePB()
    {

    }

    [MenuItem("Tool/Launcher LocalResServer", priority = 98)]
    public static void LocalResServer()
    {

    }
}
