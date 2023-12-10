
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg
{
public sealed partial class Skill : Luban.BeanBase
{
    public Skill(JSONNode _buf) 
    {
        { if(!_buf["id"].IsNumber) { throw new SerializationException(); }  Id = _buf["id"]; }
        { if(!_buf["name"].IsString) { throw new SerializationException(); }  Name = _buf["name"]; }
        { if(!_buf["cd"].IsNumber) { throw new SerializationException(); }  Cd = _buf["cd"]; }
        { var __json0 = _buf["tags"]; if(!__json0.IsArray) { throw new SerializationException(); } Tags = new System.Collections.Generic.List<SkillTags>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { SkillTags __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = (SkillTags)__e0.AsInt; }  Tags.Add(__v0); }   }
        { if(!_buf["IsInstantSkill"].IsBoolean) { throw new SerializationException(); }  IsInstantSkill = _buf["IsInstantSkill"]; }
        { if(!_buf["IsOnly"].IsBoolean) { throw new SerializationException(); }  IsOnly = _buf["IsOnly"]; }
        { var __json0 = _buf["mutexState"]; if(!__json0.IsArray) { throw new SerializationException(); } MutexState = new System.Collections.Generic.List<int>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { int __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = __e0; }  MutexState.Add(__v0); }   }
        { var __json0 = _buf["backBreakSkills"]; if(!__json0.IsArray) { throw new SerializationException(); } BackBreakSkills = new System.Collections.Generic.List<int>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { int __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = __e0; }  BackBreakSkills.Add(__v0); }   }
        { if(!_buf["valueRules"].IsString) { throw new SerializationException(); }  ValueRules = _buf["valueRules"]; }
        { var __json0 = _buf["animKeys"]; if(!__json0.IsArray) { throw new SerializationException(); } AnimKeys = new System.Collections.Generic.List<string>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { string __v0;  { if(!__e0.IsString) { throw new SerializationException(); }  __v0 = __e0; }  AnimKeys.Add(__v0); }   }
        { if(!_buf["sounds"].IsString) { throw new SerializationException(); }  Sounds = _buf["sounds"]; }
        { if(!_buf["effects"].IsString) { throw new SerializationException(); }  Effects = _buf["effects"]; }
        { if(!_buf["extraParams"].IsString) { throw new SerializationException(); }  ExtraParams = _buf["extraParams"]; }
    }

    public static Skill DeserializeSkill(JSONNode _buf)
    {
        return new Skill(_buf);
    }

    public readonly int Id;
    /// <summary>
    /// 技能名称
    /// </summary>
    public readonly string Name;
    /// <summary>
    /// 冷却时间
    /// </summary>
    public readonly int Cd;
    /// <summary>
    /// 技能标签 需要枚举
    /// </summary>
    public readonly System.Collections.Generic.List<SkillTags> Tags;
    /// <summary>
    /// 是否瞬间施放
    /// </summary>
    public readonly bool IsInstantSkill;
    /// <summary>
    /// 是否是唯一技能，玩家身上只允许存在一个技能
    /// </summary>
    public readonly bool IsOnly;
    /// <summary>
    /// 覆盖互斥状态
    /// </summary>
    public readonly System.Collections.Generic.List<int> MutexState;
    /// <summary>
    /// 可以打断该技能后摇的技能id列表
    /// </summary>
    public readonly System.Collections.Generic.List<int> BackBreakSkills;
    /// <summary>
    /// 数值
    /// </summary>
    public readonly string ValueRules;
    /// <summary>
    /// 动画key
    /// </summary>
    public readonly System.Collections.Generic.List<string> AnimKeys;
    /// <summary>
    /// 音频
    /// </summary>
    public readonly string Sounds;
    /// <summary>
    /// 特效
    /// </summary>
    public readonly string Effects;
    /// <summary>
    /// 扩展参数
    /// </summary>
    public readonly string ExtraParams;
   
    public const int __ID__ = 79944241;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
        
        
        
        
        
        
        
        
        
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "name:" + Name + ","
        + "cd:" + Cd + ","
        + "tags:" + Luban.StringUtil.CollectionToString(Tags) + ","
        + "IsInstantSkill:" + IsInstantSkill + ","
        + "IsOnly:" + IsOnly + ","
        + "mutexState:" + Luban.StringUtil.CollectionToString(MutexState) + ","
        + "backBreakSkills:" + Luban.StringUtil.CollectionToString(BackBreakSkills) + ","
        + "valueRules:" + ValueRules + ","
        + "animKeys:" + Luban.StringUtil.CollectionToString(AnimKeys) + ","
        + "sounds:" + Sounds + ","
        + "effects:" + Effects + ","
        + "extraParams:" + ExtraParams + ","
        + "}";
    }
}

}
