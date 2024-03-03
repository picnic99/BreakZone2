using Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
           var net = new NetManager();
            net.InitSocket();
            var req = new PlayerLoginReq();
            req.Username = "hyy";
            req.Password = "123456";
            req.Type = 1;
            var req2 = new PlayerLoginReq();
            req2.Username = "hyy";
            req2.Password = "123456";
            req2.Type = 2;
            net.SendProtocol(10001, req);
            net.SendProtocol(10001, req2);
        }
    }
}
