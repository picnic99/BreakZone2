using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StateSyncServer.LogicScripts.Net.Protocols;
using StateSyncServer.LogicScripts.Net.Protocols.Base;
using StateSyncServer.LogicScripts.Util;

namespace StateSyncServer.LogicScripts.Net
{
    class Server
    {

        private TcpListener server;
        private List<TcpClient> clients = new List<TcpClient>();
        private Thread listenThread;

        public void Start()
        {
            server = new TcpListener(IPAddress.Any, 8080);
            server.Start();
            listenThread = new Thread(ListenForClients);
            listenThread.Start();
            CommonUtils.Logout("服务器已启动！");

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
                clients.Add(client);
                Console.WriteLine("Client connected: " + client.Client.RemoteEndPoint.ToString());

                Thread clientThread = new Thread(HandleClientComm);
                clientThread.Start(client);
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                try
                {
                    bytesRead = 0;

                    // Block until a client sends a message  
                    bytesRead = clientStream.Read(message, 0, message.Length);

                    if (bytesRead == 0)
                    {
                        // The client disconnected from the server  
                        Console.WriteLine("Client disconnected: " + tcpClient.Client.RemoteEndPoint.ToString());
                        tcpClient.Close();
                        clients.Remove(tcpClient);
                        break;
                    }

                    // Translate the bytes received into ASCII for display purposes  
                    ASCIIEncoding encoder = new ASCIIEncoding();
                    string received = encoder.GetString(message, 0, bytesRead);
                    Received(tcpClient, received);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.StackTrace);
                    tcpClient.Close();
                    clients.Remove(tcpClient);
                    break;
                }
            }
        }

        public void Received(TcpClient client,string data)
        {
            List<string> datas = new List<string>();
            int sizeLen = 5;
            int starLen = 0;
            while (starLen < data.Length - 1)
            {
                data = data.Replace("\"", "'");
                int len = Convert.ToInt32(data.Substring(starLen, starLen + sizeLen));
                string msg = data.Substring(starLen + sizeLen, starLen + len);
                starLen += len + sizeLen;
                datas.Add(msg);
                CommonUtils.Logout("len:" + len + " data:" + msg);
            }
            foreach (var item in datas)
            {
                CommonUtils.Logout("Received protocol:" + item);
                Protocol p = JsonConvert.DeserializeObject<Protocol>(item);
                Type type = ProtocolRegClass.GetInstance().GetType(p.protocolId);
                Protocol proto = JsonConvert.DeserializeObject(item, type) as Protocol;
                //事件处理
                proto.client = client;
                GameMain.GetInstance().ProtoList.Enqueue(proto);
                //GameMain.GetInstance().Handle(client, proto);
            }
        }

        public void Send(TcpClient client,string content)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] msg = encoder.GetBytes(content);
            client.GetStream().Write(msg, 0, msg.Length);
            CommonUtils.Logout("Sent protocol:" + content);
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
