
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;


namespace cfg.ai
{
public sealed partial class UpdateDailyBehaviorProps : ai.Service
{
    public UpdateDailyBehaviorProps(ByteBuf _buf)  : base(_buf) 
    {
        SatietyKey = _buf.ReadString();
        EnergyKey = _buf.ReadString();
        MoodKey = _buf.ReadString();
        SatietyLowerThresholdKey = _buf.ReadString();
        SatietyUpperThresholdKey = _buf.ReadString();
        EnergyLowerThresholdKey = _buf.ReadString();
        EnergyUpperThresholdKey = _buf.ReadString();
        MoodLowerThresholdKey = _buf.ReadString();
        MoodUpperThresholdKey = _buf.ReadString();
    }

    public static UpdateDailyBehaviorProps DeserializeUpdateDailyBehaviorProps(ByteBuf _buf)
    {
        return new ai.UpdateDailyBehaviorProps(_buf);
    }

    public readonly string SatietyKey;
    public readonly string EnergyKey;
    public readonly string MoodKey;
    public readonly string SatietyLowerThresholdKey;
    public readonly string SatietyUpperThresholdKey;
    public readonly string EnergyLowerThresholdKey;
    public readonly string EnergyUpperThresholdKey;
    public readonly string MoodLowerThresholdKey;
    public readonly string MoodUpperThresholdKey;
   
    public const int __ID__ = -61887372;
    public override int GetTypeId() => __ID__;

    public override void ResolveRef(Tables tables)
    {
        base.ResolveRef(tables);
        
        
        
        
        
        
        
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "nodeName:" + NodeName + ","
        + "satietyKey:" + SatietyKey + ","
        + "energyKey:" + EnergyKey + ","
        + "moodKey:" + MoodKey + ","
        + "satietyLowerThresholdKey:" + SatietyLowerThresholdKey + ","
        + "satietyUpperThresholdKey:" + SatietyUpperThresholdKey + ","
        + "energyLowerThresholdKey:" + EnergyLowerThresholdKey + ","
        + "energyUpperThresholdKey:" + EnergyUpperThresholdKey + ","
        + "moodLowerThresholdKey:" + MoodLowerThresholdKey + ","
        + "moodUpperThresholdKey:" + MoodUpperThresholdKey + ","
        + "}";
    }
}

}
