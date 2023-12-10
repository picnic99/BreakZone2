
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using System.Text.Json;


namespace cfg.common
{
public partial class TbGlobalConfig
{

     private readonly common.GlobalConfig _data;

    public TbGlobalConfig(JsonElement _buf)
    {
        int n = _buf.GetArrayLength();
        if (n != 1) throw new SerializationException("table mode=one, but size != 1");
        _data = common.GlobalConfig.DeserializeGlobalConfig(_buf[0]);
    }


    /// <summary>
    /// 背包容量
    /// </summary>
     public int X1 => _data.X1;
     public int X2 => _data.X2;
     public int X3 => _data.X3;
     public int X4 => _data.X4;
     public int X5 => _data.X5;
     public int X6 => _data.X6;
     public System.Collections.Generic.List<int> X7 => _data.X7;
    
    public void ResolveRef(Tables tables)
    {
        _data.ResolveRef(tables);
    }
}

}
