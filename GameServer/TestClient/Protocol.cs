using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Protocol
    {
        public int protocolId;
        public int len;
        public byte[] data;

        public Protocol(int protocolId, int len, byte[] data)
        {
            this.protocolId = protocolId;
            this.len = len;
            this.data = data;
        }
    }
}
