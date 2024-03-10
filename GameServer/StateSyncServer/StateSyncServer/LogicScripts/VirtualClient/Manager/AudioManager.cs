using Msg;
using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System;


namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{
    public class AudioManager : Base.Manager<AudioManager>
    {
        public void Play(int playerId, AudioVO audioVO, int instanceId)
        {
            if (audioVO != null)
            {
                GameAudioPlayNtf ntf = new GameAudioPlayNtf();
                int index = -1;
                if (audioVO.IsRandomPlay())
                {
                    //随机播放
                    //TODO 使用权重变量 weight
                    index = new Random().Next(0, audioVO.audio.AudioDatas.Count);
                }
                ntf.InstanceId = instanceId;
                ntf.AudioId = audioVO.audio.Id;
                ntf.RandomIndex = index;

                LogicScripts.VO.Player player = PlayerManager.GetInstance().FindPlayer(playerId);
                System.Collections.Generic.List<LogicScripts.VO.Player> players = PlayerManager.GetInstance().GetAllPlayerInScene(player.SceneId);
                NetManager.GetInstance().SendProtoToPlayers(players, ntf);
            }
        }
    }
}