using Assets.LogicScripts.Client.Net;
using Assets.LogicScripts.Client.Net.PB;
using Assets.LogicScripts.Utils;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Assets.LogicScripts.Client.Manager
{

    class NetManager:Manager<NetManager>
    {
        Thread listenerThread; //监听数据
        IPEndPoint ipPoint;
        TcpClient client;

        //初始化
        public void Connect()
        {
            //定义服务器的IP和端口，端口与服务器对应
            var ip = IPAddress.Parse("127.0.0.1"); //可以是局域网或互联网ip，此处是本机
            ipPoint = new IPEndPoint(ip, 8080);

            client = new TcpClient(ipPoint.AddressFamily);
            client.Connect(ipPoint);


            //开启一个线程连接，必须的，否则主线程卡死
            listenerThread = new Thread(new ThreadStart(SocketReceive));
            listenerThread.Start();
        }


        void SocketReceive()
        {
            //不断接收服务器发来的数据
            while (true)
            {
                try
                {
                    Protocol proto = PbUtil.UnPack(client.GetStream());
                    CommonUtils.Logout("接收到消息：" + ProtocolId.GetProtoName(proto.protocolId)+proto.ToString());
                    //proto.client = client;
                    Event("PROTO_TEST", ProtocolId.GetProtoName(proto.protocolId) + proto.ToString());
                    if (GameMain.GetInstance() == null) continue;
                    GameMain.GetInstance().ProtoList.Enqueue(proto);

                }
                catch (Exception e)
                {
                    CommonUtils.Logout("接收出现异常: " + e.StackTrace);
                    client.Close();
                    break;
                }
            }
        }

        public void SendProtocol(IMessage protocol)
        {
            int id = RegProtocol.GetProtocolId(protocol);
            if (id > 0)
            {
                var data = protocol.ToByteArray();
                var bytes = PbUtil.Pack((uint)id, data);
                CommonUtils.Logout($"发送消息：{ProtocolId.GetProtoName(id)}len = {data.Length},protocolId = {id},data = {protocol.ToString()}");
                client.Client.Send(bytes);
            }
        }
    }
}
