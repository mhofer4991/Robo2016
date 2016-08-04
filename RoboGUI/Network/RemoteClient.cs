using GeneralLibrary;
using System;
using System.Collections.Generic;
using System.IO;
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

        public delegate void RoboStatusUpdated(RoboStatus newStatus);

        public event RoboStatusUpdated OnRoboStatusUpdated;

        public bool IsRunning
        {
            get;
            private set;
        }

        public bool Connect(string address)
        {
            this.client = new TcpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(address), PORT);

            try
            {
                client.Connect(ip);

                IsRunning = true;
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.IsBackground = true;
                t.Start(client);

                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void SendMessage(byte code, object msg)
        {
            this.SendData(code, Helper.GetBytesFromMessage(msg));
        }

        private void SendData(byte code, byte[] data)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] newMsg = Helper.BuildMessage(code, data);

                stream.Write(newMsg, 0, newMsg.Length);
                stream.Flush();
            }
            catch (IOException e)
            {
            }
            catch (SocketException e)
            {
            }
        }

        private void HandleClient(object client)
        {
            TcpClient tcp = (TcpClient)client;
            NetworkStream stream = tcp.GetStream();
            
            while (IsRunning)
            {
                if (stream.DataAvailable)
                {
                    byte[] code = new byte[1];

                    // Get code
                    if (stream.Read(code, 0, code.Length) == code.Length)
                    {
                        byte[] length = new byte[4];

                        // Get size of data
                        if (stream.Read(length, 0, length.Length) == length.Length)
                        {
                            int size = BitConverter.ToInt32(length, 0);

                            // Get data
                            byte[] data = new byte[size];
                            int bytesRead = 0;

                            while (bytesRead < data.Length)
                            {
                                int read = stream.Read(data, bytesRead, data.Length - bytesRead);
                                bytesRead += read;

                                if (read == 0)
                                {
                                    break;
                                }
                            }

                            HandleData(code[0], data);
                        }
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void HandleData(byte code, byte[] data)
        {
            // Handle data

            if (code == 5)
            {
                RoboStatus rs = Helper.GetMessageFromBytes<RoboStatus>(data);

                if (this.OnRoboStatusUpdated != null)
                {
                    this.OnRoboStatusUpdated(rs);
                }

                //this.SendData(4, Helper.GetBytesFromMessage(rs));
            }
        }
    }
}
