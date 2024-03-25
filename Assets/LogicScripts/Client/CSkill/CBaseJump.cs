using Assets.LogicScripts.Client.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogicScripts.Client.CSkill
{
    class CBaseJump : CSkillBase
    {
        public override void DoBehaviour()
        {
            var animKey0 = SkillData.GetAnimKey(0);
            var animKey1 = SkillData.GetAnimKey(1);
            var animKey2 = SkillData.GetAnimKey(2);
            AnimManager.GetInstance().PlayAnim(Player.Crt.CharacterAnimator, animKey0);
            float len = AnimManager.GetInstance().GetAnimTime(animKey0);
            TimeManager.GetInstance().AddOnceTimer(this, len, () =>
            {
                AnimManager.GetInstance().PlayAnim(Player.Crt.CharacterAnimator, animKey1);
            });

            Action<object[]> call = null;
            call = (object[] args) => {
                AnimManager.GetInstance().PlayAnim(Player.Crt.CharacterAnimator, animKey2);
                Crt.Off(Character.JUMP_END,call);
            };
            Crt.On(Character.JUMP_END, call);
            Crt.physic.Jump();
        }
    }
}
