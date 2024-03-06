using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.VO;

namespace StateSyncServer.LogicScripts.VirtualClient.Bases
{
    /// <summary>
    /// 状态的基类
    /// 目前想的是 大部分状态只是执行一些动画同时限制一些状态间的转换
    /// 特殊的状态如：SKILL ATK JUMP 这些状态 可能还会有特殊的逻辑，考虑后面做继承处理？
    /// SKILL可能会记录多种flag 比如施法前摇 后摇 什么时候允许被打断 怎么打断之类的
    /// ATK 可能会有多种攻击动作 如 多段攻击 又或者是蓄力攻击
    /// JUMP 可能需要记录一些跳跃过程中的flag
    /// 目前只考虑了最简单的状态播放动画模式
    /// </summary>
    public class State : Behaviour
    {
        //状态数据，记录一些必要的数据 支持配置
        public StateVO stateData;
        //初步用于给技能后摇切换部分 做一个记录 记录当前技能状态触发的是哪个技能 需要根据这个技能去查询哪些是支持我后摇打断的
        public int stateInfo;

        public bool stateEnd = false;
        public State() { }
        public State(Character character, StateVO vo, float time = 1)
        {
            this.character = character;
            stateData = vo;
            durationTime = time;
        }

        public override void OnEnter()
        {
            CommonUtils.Logout(character.characterData.characterName + "进入" + stateData.stateName + "状态");
            //若动画超过一段 则不播放 后续会由具体skill来播放
            AudioEventDispatcher.GetInstance().Event(MomentType.EnterState, character.characterData.id, stateData.id, "enter", character.playerId, character.InstanceId);
            if (!stateData.isSkill)
            {
                PlayStateAnim(stateData);
            }
            if (stateData.exitType == cfg.StateExitType.CHANGE_TIME_EXIT)
            {
                character.eventDispatcher.On(EventDispatcher.OPT_REDUCE_STATE_TIME, OnOptReduceTime);
            }
        }


        public void PlayStateAnim(StateVO state)
        {
            AnimCoverVO vo = character.animCoverData.GetHead(state.stateName);
            if (vo != null)
            {
                PlayAnim(vo.animName);
            }
            else
            {
                PlayAnimByState(state);
            }
        }


        /// <summary>
        /// 玩家操作减少持续时间
        /// </summary>
        private void OnOptReduceTime(object[] args)
        {
            durationTime -= durationTime * 0.1f;
        }

        public override void OnUpdate()
        {
            if (stateEnd) return;
            //无预测退出
            if (stateData.exitType == cfg.StateExitType.UNKOWN_EXIT) return;

            //计算时间退出
            //TEMP 临时处理了一下各状态的退出时间 此处应该考虑配置 以及角色的属性 如攻击速度快可以尽快结束该段攻击 或者跳跃时地面检测成功时才退出该状态 眩晕时玩家按键可以缩短退出时间之类的
            //击飞时考虑技能造成的效果都可以配置出来 如某个技能1级可以击飞1秒 5级可以5秒

            if (stateData.exitType == cfg.StateExitType.TIME_EXIT || stateData.exitType == cfg.StateExitType.CHANGE_TIME_EXIT)
            {
                durationTime -= Global.FixedFrameTimeMS;
                if (durationTime <= 0)
                {
                    character.eventDispatcher.Event(CharacterEvent.STATE_OVER, this);
                    stateEnd = true;
                }
            }
        }

        protected override void OnEnd()
        {
        }

        public override string GetDesc()
        {
            string str = "[" + stateData.stateName + "]";
            if (stateData.exitType != cfg.StateExitType.UNKOWN_EXIT)
            {
                str += " 持续" + durationTime.ToString("F2") + "s";
            }
            return str;
        }

        public override void OnExit()
        {
            if (stateData.exitType == cfg.StateExitType.CHANGE_TIME_EXIT)
            {
                character.eventDispatcher.Off(EventDispatcher.OPT_REDUCE_STATE_TIME, OnOptReduceTime);
            }
            AudioEventDispatcher.GetInstance().Event(MomentType.EnterState, character.characterData.id, stateData.id, "exit", character.playerId, character.InstanceId);
            CommonUtils.Logout(character.characterData.characterName + ":" + stateData.stateName + "结束");
        }
    }
}