using StateSyncServer.LogicScripts.VirtualClient.Actions;
using StateSyncServer.LogicScripts.VirtualClient.Buffs.newBuff;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager;
using System.Collections.Generic;

namespace StateSyncServer.LogicScripts.VirtualClient.Buffs
{
    public abstract class BuffComponent : Buff
    {
        public BuffGroup group;
        public _EventDispatcher eventDispatcher = new _EventDispatcher();
        /// <summary>
        /// buff标签
        /// </summary>
        public List<int> tags = new List<int>();

        //为该buff指定作用角色
        public override Character Character
        {
            get { return character; }
            set
            {
                character = value;
            }
        }

        public override void OnEnter()
        {

        }

        public virtual void BuffAsk(BaseAction action)
        {

        }


        protected override void OnEnd()
        {
            if (group != null)
            {
                group.RemoveBuffComponent(this);
            }
        }

        public override void OnExit()
        {

        }
    }
}