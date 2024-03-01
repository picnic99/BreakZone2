using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.Net.Protocols;

namespace StateSyncServer.LogicScripts.Manager
{
    class NetManager:Manager<NetManager>
    {
        public void SendProtocol(TcpClient client, Protocol protocol)
        {
            string json = JsonConvert.SerializeObject(protocol);
            CommonUtils.Logout("发送协议:" + json);
            if (client != null)
            {
                byte[] sendBytes = Encoding.UTF8.GetBytes(json);
                json = sendBytes.Length.ToString().PadLeft(5, '0') + json;
                sendBytes = Encoding.UTF8.GetBytes(json);
                client.GetStream().Write(sendBytes,0,sendBytes.Length);
            }
        }

        public void SendProtoToRoom(int roomId, Protocol protocol)
        {
            //协议返回
            var players = RoomManager.GetInstance().GetRoomPlayerIds(roomId);
            foreach (var id in players)
            {
                //发送数据同步协议
                NetManager.GetInstance().SendProtocol(Global.PlayerSocketMap[id], protocol);
            }
        }
    }
}
