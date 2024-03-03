using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Common
{
    class Global
    {
        //每秒帧数
        public static int FrameRate = 20;
        public static float FixedFrameTimeS = 0.05f; // 1000/20
        public static int FixedFrameTimeMS = 50; // 1000/20

        public static Dictionary<string, TcpClient> PlayerSocketMap = new Dictionary<string, TcpClient>();
    }
}
