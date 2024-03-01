using Assets.LogicScripts.Client.VO;

namespace Assets.LogicScripts.Client.Net.Protocols
{
    public class PlayerBaseInfoNtf:Protocol
    {
        public PlayerBaseInfoVO baseInfo;
        public PlayerBaseInfoNtf()
        {
            protocolId = ProtocolId.CLIENT_PLAYER_BASE_INFO_NTF;
        }
    }
}