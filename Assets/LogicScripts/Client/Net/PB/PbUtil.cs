using System;
using System.Net.Sockets;

namespace Assets.LogicScripts.Client.Net.PB
{
    public class PbUtil
    {
        private const int HEAD_BYTES = 4; //�����ֽ���
        private const int CMD_BYTES = 4;  //CMD�ֽ���

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="cmd">cmd id</param>
        /// <param name="byteArr">��Ҫ���͵�byte[]</param>
        /// <returns>���ط�װ���byte[]</returns>
        public static byte[] Pack(UInt32 cmd, byte[] byteArr)
        {
            int sendLength = byteArr.Length;
            var sendBuffer = new byte[HEAD_BYTES + CMD_BYTES + sendLength];

            // 4���ֽڵİ���
            var headBuffer = BitConverter.GetBytes((UInt32)sendLength);
            Buffer.BlockCopy(headBuffer, 0, sendBuffer, 0, HEAD_BYTES);

            // 4���ֽڵ�CMD
            var cmdBuffer = BitConverter.GetBytes(cmd);
            Buffer.BlockCopy(cmdBuffer, 0, sendBuffer, HEAD_BYTES, CMD_BYTES);

            // ������
            Buffer.BlockCopy(byteArr, 0, sendBuffer, HEAD_BYTES + CMD_BYTES, sendLength);

            return sendBuffer;
        }

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="networkStream"></param>
        public static Protocol UnPack(NetworkStream networkStream)
        {
            //����
            byte[] headBuffer = new byte[HEAD_BYTES];
            networkStream.Read(headBuffer, 0, HEAD_BYTES);
            int len = BitConverter.ToInt32(headBuffer, 0);

            // 4���ֽڵ�cmd id
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