using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.Common;
using Google.Protobuf;
using StateSyncServer.LogicScripts.Net.PB;
using StateSyncServer.LogicScripts.VO;

namespace StateSyncServer.LogicScripts.Manager
{
    class NetManager : Manager<NetManager>
    {
        public void SendProtocol(TcpClient client, IMessage protocol)
        {
            int id = RegProtocol.GetProtocolId(protocol);
            if (id > 0)
            {
                var data = protocol.ToByteArray();
                var bytes = PbUtil.Pack((uint)id, data);
                CommonUtils.Logout($"发送消息：len = {data.Length},protocolId = {id},data = {protocol.ToString()}");

                client.Client.Send(bytes);
            }
        }

        /// <summary>
        /// 发送协议给场景中的玩家
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="players"></param>
        public void SendProtoToPlayers(List<Player> players, IMessage protocol)
        {
            foreach (var item in players)
            {
                TcpClient tcpClient = Global.GetClientByPlayerId(item.playerId);
                if (tcpClient != null)
                {
                    SendProtocol(tcpClient, protocol);
                }
            }
        }
    }
}
