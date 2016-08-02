using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public static class Helper
    {
        public static T GetMessageFromBytes<T>(byte[] data)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
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
    }
}
