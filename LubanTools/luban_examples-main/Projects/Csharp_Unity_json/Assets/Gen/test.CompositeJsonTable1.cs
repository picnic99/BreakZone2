
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg.test
{
public sealed partial class CompositeJsonTable1 : Luban.BeanBase
{
    public CompositeJsonTable1(JSONNode _buf) 
    {
        { if(!_buf["id"].IsNumber) { throw new SerializationException(); }  Id = _buf["id"]; }
        { if(!_buf["x"].IsString) { throw new SerializationException(); }  X = _buf["x"]; }
    }

    public static CompositeJsonTable1 DeserializeCompositeJsonTable1(JSONNode _buf)
    {
        return new test.CompositeJsonTable1(_buf);
    }

    public readonly int Id;
    public readonly string X;
   
    public const int __ID__ = 1566207894;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "x:" + X + ","
        + "}";
    }
}

}
