using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Net.PB
{
    public class PbUtil
    {
        private const int HEAD_BYTES = 4; //包长字节数
        private const int CMD_BYTES = 4;  //CMD字节数

        /// <summary>
        /// 封包
        /// </summary>
        /// <param name="cmd">cmd id</param>
        /// <param name="byteArr">将要发送的byte[]</param>
        /// <returns>返回封装后的byte[]</returns>
        public static byte[] Pack(UInt32 cmd, byte[] byteArr)
        {
            int sendLength = byteArr.Length;
            var sendBuffer = new byte[HEAD_BYTES + CMD_BYTES + sendLength];

            // 4个字节的包长
            var headBuffer = BitConverter.GetBytes((UInt32)sendLength);
            Buffer.BlockCopy(headBuffer, 0, sendBuffer, 0, HEAD_BYTES);

            // 4个字节的CMD
            var cmdBuffer = BitConverter.GetBytes(cmd);
            Buffer.BlockCopy(cmdBuffer, 0, sendBuffer, HEAD_BYTES, CMD_BYTES);

            // 数据体
            Buffer.BlockCopy(byteArr, 0, sendBuffer, HEAD_BYTES + CMD_BYTES, sendLength);

            return sendBuffer;
        }

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="networkStream"></param>
        public static Protocol UnPack(NetworkStream networkStream)
        {
            //包长
            byte[] headBuffer = new byte[HEAD_BYTES];
            networkStream.Read(headBuffer, 0, HEAD_BYTES);
            int len = BitConverter.ToInt32(headBuffer, 0);

            // 4个字节的cmd id
            byte[] cmdBuffer = new byte[CMD_BYTES];
            networkStream.Read(cmdBuffer, 0, CMD_BYTES);
            int cmd = BitConverter.ToInt32(cmdBuffer, 0);


            byte[] buffer = new byte[len];
            int total = 0;
            while (total < len)
            {
                total += networkStream.Read(buffer, 0, len);
            }

            Protocol message = new Protocol(len, cmd, buffer);

            return message;
        }
    }
}
