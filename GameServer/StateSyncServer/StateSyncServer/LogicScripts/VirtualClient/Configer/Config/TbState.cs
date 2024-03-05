
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
public partial class TbState
{
    private readonly System.Collections.Generic.Dictionary<int, State> _dataMap;
    private readonly System.Collections.Generic.List<State> _dataList;
    
    public TbState(JsonElement _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, State>();
        _dataList = new System.Collections.Generic.List<State>();
        
        foreach(JsonElement _ele in _buf.EnumerateArray())
        {
            State _v;
            _v = State.DeserializeState(_ele);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, State> DataMap => _dataMap;
    public System.Collections.Generic.List<State> DataList => _dataList;

    public State GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public State Get(int key) => _dataMap[key];
    public State this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}
