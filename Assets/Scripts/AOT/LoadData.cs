using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using YooAsset;

public class LoadData
{
    //��ǰ����汾
    public static string PACKAGE_VERSION = "1.0";

    // ��Դϵͳ����ģʽ
# if UNITY_EDITOR
    public static EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
#else
    public static EPlayMode PlayMode = EPlayMode.HostPlayMode;
#endif
    //��Դ��������ַ
    private static string ServerUrl => GetServerURL();

    //����·��
    private static string patchPath = "BZ/CDN/" + GetPlatformPath() + "/v1.0";

    //����·��
    private static string packPath = "BZ/package/" + GetPlatformPath() + "/v1.0";

    //���ذ汾��Ϣ
    public static string localVerPath = Application.streamingAssetsPath + "/version.json";

    //��������Դ����·��
    public static string RES_SERVER_PATH = ServerUrl + patchPath;

    //��������������·��
    public static string PACKAGE_SERVER_PATH = ServerUrl + "BZ/" + packPath;

    //�������汾��Ϣ·��
    public static string SERVER_VERSION_PATH = ServerUrl + "BZ/version.json";

    //�ȸ���������
    public static string HOT_CODE = "HotUpdate";

    public static string GetPlatformPath()
    {
        string path = "";
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                path = "ios";
                break;
            case RuntimePlatform.WindowsPlayer:
                path = "pc";
                break;
            case RuntimePlatform.Android:
                path = "android";
                break;
            default:
                break;
        }
        return path;
    }

    /// <summary>
    /// ��ȡ��������ַ ����Χ����
    /// ���� ��Ҫ�ĵ�����������
    /// �ӷ�����
    /// </summary>
    /// <returns></returns>
    public static string GetServerURL()
    {
        string url = File.ReadAllText(Application.streamingAssetsPath+"/url.txt",System.Text.Encoding.UTF8);
        return url;
    }
}
