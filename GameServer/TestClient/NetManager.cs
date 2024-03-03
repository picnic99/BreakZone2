using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;

namespace TestClient
{
    class NetManager
    {

        Thread listenerThread; //监听数据
        IPEndPoint ipPoint;
        TcpClient tcplient;

        //初始化
        public void InitSocket()
        {
            //定义服务器的IP和端口，端口与服务器对应
            var ip = IPAddress.Parse("127.0.0.1"); //可以是局域网或互联网ip，此处是本机
            ipPoint = new IPEndPoint(ip, 8080);
            SocketConnet();

            //开启一个线程连接，必须的，否则主线程卡死
            listenerThread = new Thread(new ThreadStart(SocketReceive));
            listenerThread.Start();
        }

        void SocketConnet()
        {
            tcplient = new TcpClient(ipPoint.AddressFamily);
            tcplient.Connect(ipPoint);
        }

        void SocketReceive()
        {
            byte[] b = new byte[1024];
            //不断接收服务器发来的数据
            while (true)
            {
                NetworkStream networkStream = tcplient.GetStream();
                Protocol proto = PbUtil.UnPack(networkStream);
                if (proto.len > 0)
                {
                    Console.WriteLine("收到协议id:" + proto.protocolId + " 长度:" + proto.len);
   
                }

/*                int len = tcplient.Client.Receive(b);
                if (len > 0)
                {
                    Debug.Log("收到协议 ");
                    //Debug.Log("收到协议id:" + proto.protocolId + " 长度:" + proto.len);
                    //加入协议队列
                    //protocolQueue.Enqueue(proto);
                }*/
            }
        }

        public void SendProtocol(int id, IMessage data)
        {
            var bytes = PbUtil.Pack((UInt32)id, data.ToByteArray());
            // 10000  16  

            // 00000000 00000000 00100111 00010000    MSG  10000
            //     0         0        39      16
            // 00000000 00000000 00000000 00001000    LEN  8  =  bytes.Length
            //     0         0        0       8

            //大端
            //byte[] b = new byte[] { 16, 39, 0, 0, 8, 0, 0, 0, 10, 6, 233, 148, 128, 230, 136, 183 };
            //小端
            //byte[] b = new byte[] { 0, 0, 39, 16, 0, 0, 0, 8, 10, 6, 233, 148, 128, 230, 136, 183 };

            //byte[] b = new byte[] { 16, 39, 0, 0, 8, 0, 0, 0, 10, 6, 233, 148, 128, 230, 136, 183 };
            SendByte(bytes);
            Console.WriteLine("发送协议id:" + id + " 长度:" + data.ToByteArray().Length);

        }

        private void SendByte(byte[] data)
        {
            tcplient.Client.Send(data);
        }
    }
}