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

namespace StateSyncServer.LogicScripts.Manager
{
    class NetManager:Manager<NetManager>
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

        public void SendProtoToRoom(int roomId, IMessage protocol)
        {
            //协议返回
/*            var players = RoomManager.GetInstance().GetRoomPlayerIds(roomId);
            foreach (var id in players)
            {
                //发送数据同步协议
                NetManager.GetInstance().SendProtocol(Global.PlayerSocketMap[id], protocol);
            }*/
        }
    }
}
