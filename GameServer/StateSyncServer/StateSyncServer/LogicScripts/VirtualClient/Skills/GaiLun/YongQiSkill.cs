//勇气技能
using StateSyncServer.LogicScripts.VirtualClient.Base;
using StateSyncServer.LogicScripts.VirtualClient.Buffs;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;
using StateSyncServer.LogicScripts.VirtualClient.VO;

namespace StateSyncServer.LogicScripts.VirtualClient.Skills.GaiLun
{
    public class YongQiSkill : Skill
    {

        public YongQiSkill()
        {
            skillDurationTime = 5f;
        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            //AddCustomBuff(new Character[] { character }, typeof(GeDangFanJiBuff));  //格挡反击buff测试
            AddBuff(new Character[] { character }, new BuffVO("勇气减伤与护盾", skillDurationTime), (buff) =>
            {
                buff.AddBuffComponent(buff.AddPropertyBuff(skillDurationTime, new PropertyBuffVO(PropertyType.PCTDEFEND, false, 0.3f)));
                buff.AddBuffComponent(buff.AddPropertyBuff(1f, new PropertyBuffVO(PropertyType.SHIELD, false, 200f)));
            });
            //EffectManager.GetInstance().PlayEffect("Skill/YongQi", 5f, character.trans, character.trans.position, character.trans.forward, new Vector3(0.6f, 0.6f, 0.6f));
        }

        public override string GetDesc()
        {
            return "当前技能为：勇气，剩余时间为：" + skillDurationTime.ToString("F2"); ;
        }
    }
}