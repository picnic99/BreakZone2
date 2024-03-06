
using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.VirtualClient.Buffs.newBuff;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using StateSyncServer.LogicScripts.VirtualClient.States;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System;

namespace StateSyncServer.LogicScripts.VirtualClient.Base
{
    public abstract class Behaviour
    {
        public Character character;
        public float durationTime = 0;
        /// <summary>
        /// 创建后执行
        /// </summary>
        public abstract void OnEnter();
        public virtual void OnUpdate()
        {
            durationTime -= Global.FixedFrameTimeMS;
            if (durationTime <= 0)
            {
                OnEnd();
            }
        }

        /// <summary>
        /// 作用 移除后的处理
        /// </summary>
        public abstract void OnExit();

        /// <summary>
        /// 作用 用于通知外部移除
        /// </summary>
        protected abstract void OnEnd();

        public void DoDamage(Character[] targets, float value)
        {
            ActionManager.GetInstance().AddDamageAction(character, targets, value, this);
        }

        public void DoDamage(Character target, float value)
        {
            var targets = new Character[] { target };
            ActionManager.GetInstance().AddDamageAction(character, targets, value, this);
        }

        public void AddState(Character[] targets, params object[] args)
        {
            StateManager.GetInstance().AddState(character, targets, args);
        }

        public void AddState(Character target, params object[] args)
        {
            var targets = new Character[] { target };
            StateManager.GetInstance().AddState(character, targets, args);
        }


        public void AddBuff(Character[] targets, BuffVO vo, Action<BuffGroup> initCall, Action<object[]> endBuffCall = null)
        {
            BuffManager.GetInstance().AddBuffGroup(this, targets, vo, initCall, endBuffCall);
        }
        public void AddBuff(Character target, BuffVO vo, Action<BuffGroup> initCall, Action<object[]> endBuffCall = null)
        {
            var targets = new Character[] { target };
            BuffManager.GetInstance().AddBuffGroup(this, targets, vo, initCall, endBuffCall);
        }

        public void PlayAnim(string name, float translateTime = 0.15f)
        {
            if (name != "")
            {
                AnimManager.GetInstance().PlayAnim(character, name, translateTime);
            }
        }
        public void PlayAnimByState(StateVO state, float translateTime = 0.3f)
        {
            if (state != null)
            {
                AnimManager.GetInstance().PlayStateAnim(character, state, translateTime);
                //AudioManager.GetInstance().Stop(true);
                if (state.state.Type == StateType.Run)
                {
                    //AudioManager.GetInstance().Play("run", true);
                }
                if (state.state.Type == StateType.Move)
                {
                    //AudioManager.GetInstance().Play("move", true);
                }
            }
        }

        public void StopAnim(string name)
        {
            if (name != "")
            {
                //AnimManager.GetInstance().StopAnim(character, name);
            }
        }

        public void AddBuffToSelf(BuffVO vo, Action<BuffGroup> initCall, Action<object[]> endBuffCall = null)
        {
            BuffManager.GetInstance().AddBuffGroup(this, new Character[] { character }, vo, initCall, endBuffCall);
        }

        public void AddCustomBuff(Character[] targets, Type buffType, Action<object[]> endBuffCall = null)
        {
            BuffManager.GetInstance().AddCustomBuffGroup(this, targets, buffType, endBuffCall);
        }
        public void AddCustomBuffToSelf(Type buffType, Action<object[]> endBuffCall = null)
        {
            BuffManager.GetInstance().AddCustomBuffGroup(this, new Character[] { character }, buffType, endBuffCall);
        }

        public virtual string GetDesc()
        {
            return "";
        }

    }
}