using Assets.LogicScripts.Client.Entity;
using Assets.LogicScripts.Client.Manager;
using Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogicScripts.Client.CSkill
{
    class CShenPanSkill: CSkillBase
    {

        float skillDurationTime1 = 0.4f;

        float skillDurationTime2 = 0.6f;

        GameObject skillInstance = null;

        public override void DoBehaviour()
        {
            //播放动画
            var skillVO = SkillConfiger.GetInstance().GetSkillById(skillId);
            var animKey = skillVO.GetAnimKeyBySkillIndex(0);
            AnimManager.GetInstance().PlayAnim(Player.Crt.CharacterAnimator, animKey);

            TimeManager.GetInstance().AddOnceTimer(this, skillVO.GetFrontTime(0), ShowShenPanEffect);
        }

        public void ShowShenPanEffect()
        {
            skillInstance = ResourceManager.GetInstance().GetSkillInstance("ShenPan2");
            skillInstance.transform.SetParent(Player.Crt.CrtObj.transform);
            skillInstance.transform.localPosition = Vector3.zero;
            skillInstance.transform.forward = Player.Crt.CrtObj.transform.forward;
            skillInstance.transform.localScale = Vector3.zero;
            var forward = Player.Crt.CrtObj.transform.forward;

            TimeManager.GetInstance().AddFrameLoopTimer(this, 0f, skillDurationTime1 + skillDurationTime2, () =>
            {
                if (skillDurationTime1 > 0)
                {
                    skillDurationTime1 -= Time.deltaTime;
                    skillInstance.transform.localScale += new Vector3(1,1,1) * 1f * Time.deltaTime;
                    Player.Crt.CrtObj.transform.position += forward * Time.deltaTime * 5f;
                }
                else if (skillDurationTime2 > 0)
                {
                    skillInstance.transform.SetParent(null);
                    skillInstance.transform.position += forward * Time.deltaTime * 20f;
                }
            },End);
        }

        public void End()
        {
            MonoBridge.GetInstance().DestroyOBJ(skillInstance);
        }
    }
}
