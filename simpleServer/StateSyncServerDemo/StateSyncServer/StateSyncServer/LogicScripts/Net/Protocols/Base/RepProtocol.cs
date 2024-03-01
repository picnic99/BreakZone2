using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.Protocols
{
    class RepProtocol:Protocol
    {
        public int result;
        public RepProtocol(int result)
        {
            this.result = result;
        }
    }
}
