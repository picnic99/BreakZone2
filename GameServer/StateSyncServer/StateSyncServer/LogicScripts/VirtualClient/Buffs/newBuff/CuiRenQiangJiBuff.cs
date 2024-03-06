using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System;

namespace StateSyncServer.LogicScripts.VirtualClient.Buffs.newBuff
{
    /// <summary>
    /// 萃刃强击 下三个技能 有50%的概率伤害翻倍 持续10秒 
    /// </summary>
    public class CuiRenQiangJiBuff : BuffGroup
    {
        public CuiRenQiangJiBuff()
        {
            buffData = new BuffVO("萃刃强击");
        }
        public override void Init()
        {
            base.Init();
            durationTime = 10f;
            TriggerBuffComponent buff = AddTriggerBuff();
            //触发条件：监听所有的伤害类型技能
            buff.AddTrigger_SKILL_COUNT(character.characterData.GetSkillByType(cfg.SkillTags.DAMAGE_TAG), 3);
            //结束条件：触发一次结束
            buff.AddEnd_TRIGGER_COUNT();
            //结束条件：持续N秒结束
            buff.AddEnd_DURATION_TIME(durationTime);
            var random = new Random();
            //询问时回调 当Action来询问时 对action进行操作
            buff.askCall = (action) =>
            {
                //摇随机数
                int range = random.Next(1, 11);
                if (range <= 5)
                {
                    //伤害翻倍
                    float exDamage = action.value.baseValue;
                    action.value.AddExAddValue(exDamage);
                    CommonUtils.Logout("[萃刃强击] " + range + " 触发成功 伤害翻倍！");
                    RecordManager.GetInstance().AddRecord(string.Format("[{0}] 触发了[萃刃强击] 伤害翻倍 额外造成了{1}点伤害！", character.characterData.characterName, exDamage));
                }
            };

            AddBuffComponent(buff);
        }

        public override BuffFlag GetFlag()
        {
            return BuffFlag.POSITIVE;
        }

        public override string GetDesc()
        {
            if (buffComponents != null && buffComponents.Count >= 1)
            {
                return "[萃刃强击]:下三个技能 有50%的概率伤害翻倍 持续时间" + buffComponents[0].durationTime.ToString("F2") + "s";
            }
            return "";
        }
    }
}