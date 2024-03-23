using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogicScripts.Client.CSkill
{
    class CZhenYiSkill : CSkillBase
    {
        public override void DoBehaviour()
        {
            var skillVO = SkillConfiger.GetInstance().GetSkillById(skillId);
            var animKey = skillVO.GetAnimKeyBySkillIndex(0);
            AnimManager.GetInstance().PlayAnim(Player.Crt.CharacterAnimator, animKey);

            TimeManager.GetInstance().AddOnceTimer(this, skillVO.GetFrontTime(0), ShowRangeEffect);
        }
        public void ShowRangeEffect()
        {
            var instanceObj = ResourceManager.GetInstance().GetSkillInstance("ZhenYi");
            instanceObj.transform.position = CrtObj.transform.position + CrtObj.transform.forward * 7f + new Vector3(0, 0.1f,0);
            instanceObj.transform.forward = CrtObj.transform.forward;
        }
    }
}
