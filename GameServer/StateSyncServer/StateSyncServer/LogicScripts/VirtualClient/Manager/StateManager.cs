
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Manager.Base;
using System;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{
    /// <summary>
    /// ״̬������
    /// </summary>
    public class StateManager : Manager<StateManager>
    {
        internal void AddState(Character from, Character[] targets, params object[] args)
        {
            if (targets == null || targets.Length <= 0) return;
            foreach (var target in targets)
            {
                target.eventDispatcher.Event(CharacterEvent.CHANGE_STATE, args);
            }
        }
    }
}