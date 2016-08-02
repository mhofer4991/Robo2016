using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Network
{
    public class RemoteClient
    {
        public const int PORT = 4001;

        private TcpClient client;

        public RemoteClient()
        {
        }

        public bool Connect(string address)
        {
            this.client = new TcpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(address), PORT);

            try
            {
                client.Connect(ip);

                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(client);

                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        private void HandleClient(object client)
        {
            TcpClient tcp = (TcpClient)client;
            NetworkStream stream = tcp.GetStream();
            
        }
    }
}
