using Assets.LogicScripts.Client.Net;
using Assets.LogicScripts.Client.Net.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Assets.LogicScripts.Client.Manager
{

    class NetManager:Manager<NetManager>
    {
        Socket client;

        public void Connect()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ip, int.Parse("8080"));
            client.Connect(endPoint);
            Thread thread = new Thread(ReciveMsg);
            thread.IsBackground = true;
            thread.Start(client);
            Debug.Log("Hello Server");
        }

        /// <summary>
        /// 客户端接收到服务器发送的消息
        /// </summary>
        /// <param name="o">客户端</param>
        void ReciveMsg(object o)
        {
            Socket client = o as Socket;
            while (true)
            {
                ///定义客户端接收到的信息大小
                byte[] arrList = new byte[4096];
                ///接收到的信息大小(所占字节数)
                int length = client.Receive(arrList);
                string json = Encoding.UTF8.GetString(arrList, 0, length);
                Debug.Log(System.DateTime.Now.ToString("HH:mm:ss:fff") + " - 服务器发来的原始数据:" + json);
                json = json.Replace('\'', '"');
                if (json.IndexOf("{") == -1) continue;

                int sizeLen = 5;
                int starLen = 0;
                while (starLen < json.Length - 1)
                {
                    int len = Convert.ToInt32(json.Substring(starLen, sizeLen));
                    string msg = json.Substring(starLen + sizeLen, len);
                    starLen += len + sizeLen;
                    Protocol p = JsonUtility.FromJson<Protocol>(msg);
                    Type type = ProtocolRegClass.GetInstance().GetType(p.protocolId);
                    Protocol proto = JsonConvert.DeserializeObject(msg, type) as Protocol;

                    GameMain.GetInstance().ProtoList.Enqueue(proto);
                }
            }
        }

        private void SendMsg(string msg)
        {
            byte[] sendBytes = Encoding.UTF8.GetBytes(msg);
            msg = sendBytes.Length.ToString().PadLeft(5, '0') + msg;
            sendBytes = Encoding.UTF8.GetBytes(msg);
            //把二进制的消息发出去
            client.Send(sendBytes);
        }

        public void SendProtocol(Protocol proto)
        {
            string json = JsonConvert.SerializeObject(proto);
            SendMsg(json);
        }
    }
}
