using StateSyncServer.LogicScripts.VirtualClient.Base;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.States;
using StateSyncServer.LogicScripts.VirtualClient.VO;

namespace StateSyncServer.LogicScripts.VirtualClient.Buffs.newBuff
{
    /// <summary>
    /// 固若金汤 当受到眩晕禁锢时，提升50%的伤害减免 持续5s
    /// </summary>
    public class GuRuoJingTangBuff : BuffGroup
    {
        public GuRuoJingTangBuff()
        {
            buffData = new BuffVO("固若金汤");
        }

        public override void Init()
        {
            base.Init();
            durationTime = float.MaxValue;
            TriggerBuffComponent buff = AddTriggerBuff();
            buff.AddTrigger_STATE_CHANGE(character, new string[] { StateType.Dizzy, StateType.Trap });
            buff.AddEnd_TRIGGER_COUNT();
            buff.doBuffCall = (args) =>
            {
                AddBuff(new Character[] { character }, new BuffVO("固若金汤免伤", 5f), (buff1) =>
             {
                 buff1.AddBuffComponent(buff1.AddPropertyBuff(5f, new PropertyBuffVO(PropertyType.PCTDEFEND, false, 0.5f)));
             });
            };
            AddBuffComponent(buff);
        }
        public override BuffFlag GetFlag()
        {
            return BuffFlag.NAGATIVE;
        }

        public override string GetDesc()
        {
            return "[固若金汤]:当受到眩晕禁锢时，提升50%的伤害减免 持续时间" + durationTime.ToString("F2") + "s";
        }

    }
}