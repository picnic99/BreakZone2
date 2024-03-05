
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using System.Text.Json;


namespace cfg
{
public sealed partial class Animation : Luban.BeanBase
{
    public Animation(JsonElement _buf) 
    {
        Key = _buf.GetProperty("key").GetString();
        AnimType = _buf.GetProperty("animType").GetInt32();
        { var __json0 = _buf.GetProperty("clips"); Clips = new System.Collections.Generic.List<AnimClipData>(__json0.GetArrayLength()); foreach(JsonElement __e0 in __json0.EnumerateArray()) { AnimClipData __v0;  __v0 = AnimClipData.DeserializeAnimClipData(__e0);  Clips.Add(__v0); }   }
        FrontTime = _buf.GetProperty("frontTime").GetSingle();
        BackTime = _buf.GetProperty("backTime").GetSingle();
        { var __json0 = _buf.GetProperty("eventTimes"); EventTimes = new System.Collections.Generic.List<string>(__json0.GetArrayLength()); foreach(JsonElement __e0 in __json0.EnumerateArray()) { string __v0;  __v0 = __e0.GetString();  EventTimes.Add(__v0); }   }
        ValidLength = _buf.GetProperty("validLength").GetSingle();
    }

    public static Animation DeserializeAnimation(JsonElement _buf)
    {
        return new Animation(_buf);
    }

    public readonly string Key;
    public readonly int AnimType;
    /// <summary>
    /// animPath
    /// </summary>
    public readonly System.Collections.Generic.List<AnimClipData> Clips;
    public readonly float FrontTime;
    public readonly float BackTime;
    public readonly System.Collections.Generic.List<string> EventTimes;
    public readonly float ValidLength;
   
    public const int __ID__ = -1172489372;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
        foreach (var _e in Clips) { _e?.ResolveRef(tables); }
        
        
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "key:" + Key + ","
        + "animType:" + AnimType + ","
        + "clips:" + Luban.StringUtil.CollectionToString(Clips) + ","
        + "frontTime:" + FrontTime + ","
        + "backTime:" + BackTime + ","
        + "eventTimes:" + Luban.StringUtil.CollectionToString(EventTimes) + ","
        + "validLength:" + ValidLength + ","
        + "}";
    }
}

}
