using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.Net.PB;
using StateSyncServer.LogicScripts.Util;

namespace StateSyncServer.LogicScripts.Net
{
    class Server
    {
        private TcpListener server;
        private List<TcpClient> clients = new List<TcpClient>();
        private Thread listenThread;
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        private int BUFF_SIZE = 4096;


        public void Start()
        {
            server = new TcpListener(IPAddress.Any, 8080);
            server.Start();
            listenThread = new Thread(ListenForClients);
            listenThread.Start();
            CommonUtils.Logout("服务器已启动");

        }

        public void RunServer()
        {
            Start();
        }

        private void ListenForClients()
        {
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                client.ReceiveBufferSize = BUFF_SIZE;
                client.SendBufferSize = BUFF_SIZE;
                clients.Add(client);
                Console.WriteLine("客户端连接: " + client.Client.RemoteEndPoint.ToString());

                Thread clientThread = new Thread(Receive);
                clientThread.Start(client);
            }
        }

        private void Receive(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            while (true)
            {
                try
                {
                    Protocol proto = PbUtil.UnPack(tcpClient.GetStream());
                    CommonUtils.Logout("接收到消息："  + ProtocolId.GetProtoName(proto.protocolId) + proto.ToString());
                    proto.client = tcpClient;
                    GameMain.GetInstance().ProtoList.Enqueue(proto);

                    //NetManager.GetInstance().SendProtocol(tcpClient, proto.GetDataInstance());
                }
                catch (Exception e)
                {
                    Console.WriteLine("接收出现异常: " + e.StackTrace);
                    tcpClient.Close();
                    clients.Remove(tcpClient);
                    break;
                }
            }
        }

        public void Send(TcpClient client, byte[] data)
        {
            client.Client.Send(data);
        }

        public void Stop()
        {
            server.Stop();
            foreach (TcpClient client in clients)
            {
                client.Close();
            }
            clients.Clear();
            listenThread.Join();
        }
    }
}
