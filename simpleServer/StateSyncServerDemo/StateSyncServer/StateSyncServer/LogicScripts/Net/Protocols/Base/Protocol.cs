using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.Protocols
{
    class Protocol
    {
        public int protocolId = 0;
        [NonSerialized]
        public TcpClient client;
    }
}
