using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public static class Helper
    {
        public static T GetMessageFromBytes<T>(byte[] data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        public static byte[] GetBytesFromMessage(object message)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        }

        public static byte[] BuildMessage(byte code, byte[] data)
        {
            List<byte> message = new List<byte>();

            message.Add(code);
            message.AddRange(BitConverter.GetBytes(data.Length));
            message.AddRange(data);

            return message.ToArray();
        }

        public static bool IsConnected(this Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }
    }
}
