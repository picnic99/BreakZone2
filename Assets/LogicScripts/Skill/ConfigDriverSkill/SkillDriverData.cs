public class SkillDriverData
{
    /// <summary>
    /// 技能持续的总时长
    /// </summary>
    public float totalTime;
    /// <summary>
    /// 技能前摇时间点
    /// </summary>
    public float frontTime;
    /// <summary>
    /// 技能后摇时间点
    /// </summary>
    public float backTime;
    /// <summary>
    /// action触发
    /// </summary>
    public ActionDriver[] actionDrivers;

    public AnimDriver[] animDriver;

    public EffectDriver[] effectDriver;

    public SoundDriver[] soundDriver;

    public SkillDriverData(float totalTime, float frontTime, float backTime, ActionDriver[] actionDrivers, AnimDriver[] animDriver, EffectDriver[] effectDriver, SoundDriver[] soundDriver)
    {
        this.totalTime = totalTime;
        this.frontTime = frontTime;
        this.backTime = backTime;
        this.actionDrivers = actionDrivers;
        this.animDriver = animDriver;
        this.effectDriver = effectDriver;
        this.soundDriver = soundDriver;
    }
}