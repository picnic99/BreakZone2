using StateSyncServer.LogicScripts.Manager;
using StateSyncServer.LogicScripts.Net;
using StateSyncServer.LogicScripts.Net.PB;
using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StateSyncServer
{
    class Run
    {
        static void Main(string[] args)
        {
            //RegProtocol.Init();
            //new Server().RunServer();
            //GameMain.GetInstance().Run();
            //MatrixUtils.Test();
            BoxCollider.Test();
            Console.ReadLine();
        }
    }
}
