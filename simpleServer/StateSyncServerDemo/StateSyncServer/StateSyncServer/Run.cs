using StateSyncServer.LogicScripts.Net;
using StateSyncServer.LogicScripts.Net.Protocols;
using StateSyncServer.LogicScripts.Util;
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
            new Server().RunServer();
            GameMain.GetInstance().Run();
            //MatrixUtils.Test();
        }
    }
}
