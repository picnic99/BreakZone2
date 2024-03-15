using Assets.LogicScripts.Client.Manager;
using Assets.LogicScripts.Client.Net.PB;
using Cysharp.Threading.Tasks;
using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TMP_Dropdown;
using ActionManager = Assets.LogicScripts.Client.Manager.ActionManager;

public class ProtoTest : MonoBehaviour
{
    public Button loginBtn;
    public Button sendBtn;
    public Button atkBtn;
    public TMPro.TMP_InputField paramInput;
    public TMPro.TMP_InputField ctnInput;
    public TMPro.TMP_InputField fixedInput;
    public TMPro.TextMeshProUGUI BackLog;
    public TMPro.TMP_Dropdown protoSelect;
    public GameObject tips;
    public TMPro.TextMeshProUGUI tipsTxt;
    private Assets.LogicScripts.Client.Manager.ActionManager mgr => Assets.LogicScripts.Client.Manager.ActionManager.GetInstance();


    private string lastBack = "";
    List<OptionData> opList = new List<OptionData>();
    List<int> opValueList = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        RegProtocol.Init();
        Init();
        Assets.LogicScripts.Client.Manager.NetManager.GetInstance().Connect();
    }

    // Update is called once per frame
    void Update()
    {
        BackLog.text = lastBack;
    }

    public void Init()
    {
        NetManager.GetInstance().On("PROTO_TEST", ShowProtoBack);
        loginBtn.onClick.AddListener(OneKeyLogin);
        sendBtn.onClick.AddListener(SendProto);
        atkBtn.onClick.AddListener(OnAtk);
        //初始化协议列表
        foreach (var item in RegProtocol.GetDic())
        {
            OptionData op = new OptionData();
            op.text = ProtocolId.GetProtoName(item.Key);
            opList.Add(op);
            opValueList.Add(item.Key);
        }
        protoSelect.AddOptions(opList);
        protoSelect.onValueChanged.AddListener(ShowSelect);
    }

    async public void SendProto()
    {
        var param = paramInput.text;
        object[] pObj = new object[0];
        if (param.Length > 0)
        {
            var ps = param.Split(';');
            pObj = new object[ps.Length];
            for (int i = 0; i < ps.Length; i++)
            {
                pObj[i] = ps[i];
            }

        }

        List<string> fieldList = new List<string>();
        List<Type> fieldTypeList = new List<Type>();
        Type type = RegProtocol.GetProtocolType(opValueList[protoSelect.value]);
        IMessage msg = (IMessage)type.Assembly.CreateInstance(type.FullName);
        var fields = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        foreach (var item in fields)
        {
            if (item.CanWrite)
            {
                fieldList.Add(item.Name);
                fieldTypeList.Add(item.PropertyType);
            }
        }

        for (int i = 0; i < pObj.Length; i++)
        {
            object convertedValue = Convert.ChangeType(pObj[i], fieldTypeList[i]);
            SetPropertyValue(msg, fieldList[i], convertedValue);
        }

        int ctn = Convert.ToInt32(ctnInput.text);
        float time = Convert.ToSingle(fixedInput.text);

        while (ctn >= 1)
        {
            NetManager.GetInstance().SendProtocol(msg);
            ctn--;
            await UniTask.WaitForSeconds(time);
        }
    }

    public static void SetPropertyValue(object obj, string propertyName, object value)
    {
        // 获取属性的PropertyInfo  
        PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (propertyInfo == null)
        {
            // 属性不存在，可以记录日志或抛出异常  
            Console.WriteLine($"Warning: Property {propertyName} not found on type {obj.GetType().FullName}");
            return;
        }

        // 检查属性是否可写  
        if (!propertyInfo.CanWrite)
        {
            // 属性不可写，可以记录日志或抛出异常  
            Console.WriteLine($"Warning: Property {propertyName} is not writable on type {obj.GetType().FullName}");
            return;
        }

        // 检查值的类型是否与属性类型匹配（可选）  
/*        if (!propertyInfo.PropertyType.IsAssignableFrom(value.GetType()))
        {
            // 类型不匹配，可以记录日志、尝试转换或抛出异常  
            Console.WriteLine($"Warning: Type mismatch for property {propertyName}. Expected {propertyInfo.PropertyType.FullName}, got {value.GetType().FullName}");
            return;
        }*/

        // 设置属性值  
        propertyInfo.SetValue(obj, value, null);
    }

    public void OnAtk()
    {
        int index = -1;
        for (int i = 0; i < opList.Count; i++)
        {
            if(opList[i].text == ProtocolId.GetProtoName(ProtocolId.CLIENT_GAME_PLAYER_OPT_CMD_REQ))
            {
                index = i;
                break;
            }
        }
        if(index >= 0)
        {
            paramInput.text = "1;"+StateType.DoAtk + ";1;8003";
            protoSelect.value = index;
            ShowTips("攻击协议设置成功！");
        }


    }

    async public void OneKeyLogin()
    {
        mgr.SendLoginReq("1", "1", 1);
        await UniTask.Delay(100);
        mgr.SendSelectCrtReq(1);
        await UniTask.Delay(100);
        mgr.SendCanEnterSceneReq(1);
        await UniTask.Delay(100);
        mgr.SendEnterSceneReq(1);
        ShowTips("登录成功！");
    }

    public void ShowSelect(int index)
    {
        ShowTips(opList[index].text);
    }

    public void ShowTips(string str)
    {
        tips.SetActive(true);
        tipsTxt.text = str;
        Invoke("HideTips", 1f);
    }

    public void HideTips()
    {
        tips.SetActive(false);
    }

    public void ShowProtoBack(object[] args)
    {
        string msg = (string)args[0];
        lastBack = msg;
    }
}
