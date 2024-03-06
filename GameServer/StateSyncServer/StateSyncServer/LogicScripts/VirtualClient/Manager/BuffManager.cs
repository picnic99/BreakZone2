
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Buffs;
using StateSyncServer.LogicScripts.VirtualClient.Buffs.newBuff;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager.Base;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{
    /// <summary>
    /// Buff管理器
    /// 负责处理添加删除buff的行为
    /// </summary>
    public class BuffManager : Manager<BuffManager>
    {
        public void RemoveBuff(Character character)
        {

        }

        public override void AddEventListener()
        {
            base.AddEventListener();
            //CharacterManager.Eventer.On(CharacterManager.REMOVE_CHARACTER, OnRemoveAllBuffByCharacter);
        }

        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
            //CharacterManager.Eventer.Off(CharacterManager.REMOVE_CHARACTER, OnRemoveAllBuffByCharacter);
        }

        /// <summary>
        /// 添加一个自定义buff组
        /// 该方法需要实现一个buff类
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="buff"></param>
        public void AddCustomBuffGroup(Behaviour from, Character[] targets, Type buff, Action<object[]> endBuffCall)
        {
            foreach (var target in targets)
            {
                var inst = (BuffGroup)CreateBuff(buff, null);
                inst.from = from;
                inst.endBuffCall = endBuffCall;
                inst.Character = target;
            }
        }

        /// <summary>
        /// 添加一个buff组
        /// 可以在此基础上叠加各种buff
        /// 该方法可以避免继承一个类
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="buff"></param>
        /// <param name="vo"></param>
        public void AddBuffGroup(Behaviour from, Character[] targets, BuffVO vo, Action<BuffGroup> initCall, Action<object[]> endBuffCall)
        {
            foreach (var target in targets)
            {
                var inst = GetBuffGroup(vo);
                inst.initCall = initCall;
                inst.endBuffCall = endBuffCall;
                inst.from = from;
                inst.Character = target;
            }
        }

        private BuffGroup GetBuffGroup(BuffVO vo = null)
        {
            BuffGroup inst = (BuffGroup)CreateBuff(typeof(BuffGroup), null);
            if (vo != null) inst.buffData = vo;
            return inst;
        }


        /// <summary>
        /// 创建一个BUFF
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Buff CreateBuff(Type buff, params object[] args)
        {
            Buff buffInstance = null;
            if (args == null || args.Length <= 0)
            {
                buffInstance = (Buff)buff.Assembly.CreateInstance(buff.Name);
            }
            else
            {
                buffInstance = (Buff)buff.Assembly.CreateInstance(buff.Name, false, BindingFlags.Default, null, args, null, null);
            }
            return buffInstance;
        }

        //移除目标角色身上的所有BUFF
        public void RemoveAllBuffByCharacter(Character target)
        {
            foreach (var item in target.BuffBehaviour)
            {
                item.OnDestroy();
            }
        }

        public void OnRemoveAllBuffByCharacter(object[] args)
        {
            Character c = args[0] as Character;
            RemoveAllBuffByCharacter(c);
        }

        internal void RemoveAllBuffFromSkill(Character character, Skill yongQiSkill)
        {

        }
    }
}