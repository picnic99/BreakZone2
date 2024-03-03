using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Assets.LogicScripts.Client.Net.PB
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

        public T GetDataInstance<T>() where T : IMessage<T>
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
