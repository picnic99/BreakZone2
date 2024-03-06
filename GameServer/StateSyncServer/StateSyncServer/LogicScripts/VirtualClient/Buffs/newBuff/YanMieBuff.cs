using StateSyncServer.LogicScripts.VirtualClient.Base;
using StateSyncServer.LogicScripts.VirtualClient.VO;

namespace StateSyncServer.LogicScripts.VirtualClient.Buffs.newBuff
{
    public class YanMieBuff : BuffGroup
    {
        public YanMieBuff()
        {
            buffData = new BuffVO("湮灭");
        }

        public override void Init()
        {
            base.Init();
            durationTime = 4f;
            AddBuffComponent(AddPropertyBuff(durationTime, new PropertyBuffVO(PropertyType.ATK, false, 100), new PropertyBuffVO(PropertyType.ATKSPEED, false, 0.5f)));
        }

        public override BuffFlag GetFlag()
        {
            return BuffFlag.POSITIVE;
        }

        public override string GetDesc()
        {
            return "[湮灭]:攻击力+100 攻击速度提升50% 持续时间" + durationTime.ToString("F2") + "s";
        }
    }
}