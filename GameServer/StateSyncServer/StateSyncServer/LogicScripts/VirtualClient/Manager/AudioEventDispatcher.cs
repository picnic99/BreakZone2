using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Base;
using StateSyncServer.LogicScripts.VirtualClient.Configer;
using StateSyncServer.LogicScripts.VirtualClient.Skills.Base;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{
    public class AudioEventDispatcher : VirtualClient.Base.Singleton<AudioEventDispatcher>
    {
        public EventDispatcher dispatcher;

        public void On(string eventName, Action<object[]> callback)
        {
            dispatcher.On(eventName, callback);
        }

        public void Off(string eventName, Action<object[]> callback)
        {
            dispatcher.Off(eventName, callback);
        }

        public void Event(string eventName, params object[] args)
        {
            dispatcher.Event(eventName, args);
        }

        public void Init()
        {
            dispatcher = new EventDispatcher();
            dispatcher.On(MomentType.DoSkill, Handle_DoSkill);
            dispatcher.On(MomentType.EnterState, Handle_EnterState);
        }

        private void Handle_DoSkill(object[] args)
        {
            Skill skill = (Skill)args[0];
            string keyword = (string)args[1];
            int playerId = (int)args[2];
            int instanceId = (int)args[3];

            AudioVO vo = AudioConfiger.GetInstance().GetAudioData(0, skill.skillData.Id, 0, keyword);
            if (vo != null)
            {
                AudioManager.GetInstance().Play(playerId, vo, instanceId);
            }
            else
            {
                CommonUtils.Logout("AudioConfiger配置有误！！！");
            }
        }

        private void Handle_EnterState(object[] args)
        {
            int crtId = (int)args[0];
            int stateId = (int)args[1];
            string keyword = (string)args[2];
            int playerId = (int)args[3];
            int instanceId = (int)args[4];
            AudioVO vo = AudioConfiger.GetInstance().GetAudioData(crtId, 0, stateId, keyword);
            if (vo != null)
            {
                AudioManager.GetInstance().Play(playerId, vo, instanceId);
            }
            else
            {
                CommonUtils.Logout($"AudioConfiger配置有误！！！: {crtId}_0_{stateId}_{keyword}");
            }
        }
    }
}