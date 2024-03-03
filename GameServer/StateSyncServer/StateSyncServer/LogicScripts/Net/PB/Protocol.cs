using Google.Protobuf;
using Google.Protobuf.Reflection;
using Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.PB
{
    [System.Serializable]
    public class Protocol
    {
        public int protocolId;
        public int len;
        public byte[] data;

        public TcpClient client;

        public Protocol(int len, int protocolId, byte[] data)
        {
            this.protocolId = protocolId;
            this.len = len;
            this.data = data;
        }

        public override string ToString()
        {

            return $"len = {len},protocolId = {protocolId},data = {GetDataInstance().ToString()}";
        }

        public T GetDataInstance<T>() where T:IMessage<T>
        {
            Type type = RegProtocol.GetProtocolType(protocolId);
            MessageParser<T> parser = (MessageParser<T>)type.GetProperty("Parser").GetValue(null);
            return parser.ParseFrom(data);
        }

        public IMessage GetDataInstance()
        {
            Type type = RegProtocol.GetProtocolType(protocolId);
            MessageParser parser = (MessageParser)type.GetProperty("Parser").GetValue(null);
            return parser.ParseFrom(data);
        }
    }
}
