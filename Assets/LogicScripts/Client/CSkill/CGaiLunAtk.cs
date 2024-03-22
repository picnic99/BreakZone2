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
    class CGaiLunAtk : CSkillBase
    { 

        public override void DoBehaviour()
        {
            //播放动画
            var skillVO = SkillConfiger.GetInstance().GetSkillById(skillId);
            var animKey = skillVO.GetAnimKeyBySkillIndex(stageNum);
            AnimManager.GetInstance().PlayAnim(Player.Crt.CharacterAnimator, animKey);

            TimeManager.GetInstance().AddOnceTimer(this, skillVO.GetFrontTime(stageNum),ShowAtkEffect);
 
        }

        public void ShowAtkEffect()
        {
            //生成特效
            GameObject skillInstance = null;
            skillInstance = ResourceManager.GetInstance().GetSkillInstance("GaiLunAtk");
            skillInstance.transform.SetParent(Player.Crt.CrtObj.transform);
            skillInstance.transform.localPosition = Vector3.zero;
            skillInstance.transform.forward = Player.Crt.CrtObj.transform.forward;
            GameObject curAtk = skillInstance.transform.Find(stageNum + "").gameObject;
            curAtk.SetActive(true);

            TimeManager.GetInstance().AddOnceTimer(this, 1, () =>
            {
                MonoBridge.GetInstance().DestroyOBJ(skillInstance);
            });
        }
    }
}
