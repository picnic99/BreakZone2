using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;

namespace StateSyncServer.LogicScripts.VirtualClient.Skills
{
    public class BaseRoll : Skill
    {
        public BaseRoll() : base()
        {
            stateDurationTime = 1f;
            skillDurationTime = 1.2f;
        }

        public override void OnEnter()
        {
            //播放动画前 考虑动画覆盖问题
            //动画覆盖分为两种 1 状态动画 2 技能动画
            AnimCoverVO vo = character.animCoverData.GetHead(belongState);
            if (vo != null)
            {
                PlayAnim(vo.animName);
            }
            else
            {

                PlayAnim(skillData.GetAnimKey(GetAnimIndexByInput()));
            }
            base.OnEnter();
        }

        private int GetAnimIndexByInput()
        {
            var input = character.input.InputData;
            int animIndex = 0;
            if (input.Y >= 0)
            {
                //前
                animIndex = 0;
            }
            else
            {
                //后
                animIndex = 1;
            }
            if (input.X > 0)
            {
                //右
                animIndex = 3;
            }
            else if (input.X < 0)
            {
                //左
                animIndex = 2;
            }
            return animIndex;

        }
    }
}