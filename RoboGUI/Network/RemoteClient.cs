using GeneralLibrary;
using RoboInput;
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
    public class RemoteClient : IRemoteInputSender
    {
        public const int PORT = 4001;

        public const byte MSGCODE_REMOTE_CONTROL_INPUT = 1;

        public const byte MSGCODE_ROBOT_STATUS_UPDATE = 5;

        public const byte MSGCODE_ROBOT_CALIBRATE_REQUEST = 6;

        public const byte MSGCODE_ROBOT_TRAVEL_ROUTE_REQUEST = 7;

        public const byte MSGCODE_ROBOT_TRAVEL_ROUTE_RESPONSE = 8;

        public const byte MSGCODE_ROBOT_SCAN_MAP_UPDATE = 9;

        public const byte MSGCODE_ROBOT_CANCEL_ROUTE_REQUESET = 10;

        public const byte MSGCODE_ROBOT_START_MANUAL_SCAN_MODE = 11;

        public const byte MSGCODE_ROBOT_EXIT_MANUAL_SCAN_MODE = 12;

        public const byte MSGCODE_ROBOT_START_AUTO_SCAN_MODE = 13;

        public const byte MSGCODE_ROBOT_EXIT_AUTO_SCAN_MODE = 14;

        public const byte MSGCODE_ROBOT_LOG = 15;

        private TcpClient client;

        public RemoteClient()
        {
            this.client = new TcpClient();
        }

        public delegate void RobotConnected();

        public delegate void RobotDisconnected();

        public delegate void ConnectionFailed();

        public delegate void RoboStatusUpdated(RoboStatus newStatus);

        public delegate void TravelResponseReceived(TravelResponse response);

        public delegate void MapUpdated(Map map);

        public delegate void LogReceived(string text);

        public event RobotConnected OnRobotConnected;

        public event RobotDisconnected OnRobotDisconnected;

        public event ConnectionFailed OnConnectionFailed;

        public event RoboStatusUpdated OnRoboStatusUpdated;

        public event TravelResponseReceived OnTravelResponseReceived;

        public event MapUpdated OnMapUpdated;

        public event LogReceived OnLogReceived;

        public bool IsRunning
        {
            get;
            private set;
        }

        public bool IsConnected
        {
            get;
            private set;
        }

        public void Connect(string address)
        {
            this.client = new TcpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(address), PORT);
            IsConnected = false;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    client.Connect(ip);

                    IsRunning = true;
                    IsConnected = true;

                    Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                    t.IsBackground = true;
                    t.Start(client);

                    if (this.OnRobotConnected != null)
                    {
                        this.OnRobotConnected();
                    }
                }
                catch (SocketException)
                {
                    if (this.OnConnectionFailed != null)
                    {
                        this.OnConnectionFailed();
                    }
                }
            });
        }

        public void Stop()
        {
            try
            {
                this.client.Close();
            }
            catch (IOException e)
            {
            }
            catch (SocketException e)
            {
            }

            IsRunning = false;
            IsConnected = false;

            if (this.OnRobotDisconnected != null)
            {
                this.OnRobotDisconnected();
            }
        }

        //
        // Send methods
        //

        public void SendInput(ControlInput controlInput)
        {
            this.SendMessage(MSGCODE_REMOTE_CONTROL_INPUT, controlInput);
        }

        public void SendCalibratingRequest()
        {
            this.SendMessage(MSGCODE_ROBOT_CALIBRATE_REQUEST, MSGCODE_ROBOT_CALIBRATE_REQUEST);
        }

        public void SendTravelRequest(int id, Map map, Route route)
        {
            this.SendMessage(MSGCODE_ROBOT_TRAVEL_ROUTE_REQUEST, new TravelRequest(id, map, route));
        }

        public void SendCancelRouteRequest()
        {
            this.SendMessage(MSGCODE_ROBOT_CANCEL_ROUTE_REQUESET, MSGCODE_ROBOT_CANCEL_ROUTE_REQUESET);
        }

        public void StartManualScanMode()
        {
            this.SendMessage(MSGCODE_ROBOT_START_MANUAL_SCAN_MODE, MSGCODE_ROBOT_START_MANUAL_SCAN_MODE);
        }

        public void ExitManualScanMode()
        {
            this.SendMessage(MSGCODE_ROBOT_EXIT_MANUAL_SCAN_MODE, MSGCODE_ROBOT_EXIT_MANUAL_SCAN_MODE);
        }

        public void StartAutoScanMode()
        {
            this.SendMessage(MSGCODE_ROBOT_START_AUTO_SCAN_MODE, MSGCODE_ROBOT_START_AUTO_SCAN_MODE);
        }

        public void ExitAutoScanMode()
        {
            this.SendMessage(MSGCODE_ROBOT_EXIT_AUTO_SCAN_MODE, MSGCODE_ROBOT_EXIT_AUTO_SCAN_MODE);
        }

        public void SendMessage(byte code, object msg)
        {
            this.SendData(code, Helper.GetBytesFromMessage(msg));
        }

        private void SendData(byte code, byte[] data)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (this.IsConnected)
                    {
                        NetworkStream stream = client.GetStream();
                        byte[] newMsg = Helper.BuildMessage(code, data);

                        stream.Write(newMsg, 0, newMsg.Length);
                        stream.Flush();
                    }
                }
                catch (IOException e)
                {
                    this.Stop();
                }
                catch (SocketException e)
                {
                    this.Stop();
                }
            });
        }

        //
        // Handle methods
        //

        private void HandleClient(object client)
        {
            TcpClient tcp = (TcpClient)client;
            NetworkStream stream = tcp.GetStream();
            bool okay = true;
            
            try
            {
                while (IsRunning && okay)
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

                                try
                                {
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

                                    if (!HandleData(code[0], data))
                                    {
                                        // Handling data was not successful.
                                        okay = false;
                                    }
                                }
                                catch (OutOfMemoryException)
                                {
                                    // okay = false?????
                                }
                            }
                        }
                    }
                    else if (!Helper.IsConnected(tcp.Client))
                    {
                        okay = false;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }

                if (!okay)
                {
                    this.Stop();
                }
            }
            catch (IOException e)
            {
                this.Stop();
            }
            catch (SocketException e)
            {
                this.Stop();
            }
        }

        private bool HandleData(byte code, byte[] data)
        {
            // Handle data

            if (code == MSGCODE_ROBOT_STATUS_UPDATE)
            {
                RoboStatus rs = Helper.GetMessageFromBytes<RoboStatus>(data);

                if (rs != null)
                {
                    if (this.OnRoboStatusUpdated != null)
                    {
                        this.OnRoboStatusUpdated(rs);
                    }
                }

                //this.SendData(4, Helper.GetBytesFromMessage(rs));
            }
            else if (code == MSGCODE_ROBOT_TRAVEL_ROUTE_RESPONSE)
            {
                TravelResponse response = Helper.GetMessageFromBytes<TravelResponse>(data);

                if (this.OnTravelResponseReceived != null)
                {
                    this.OnTravelResponseReceived(response);
                }
            }
            else if (code == MSGCODE_ROBOT_SCAN_MAP_UPDATE)
            {
                Map m = Helper.GetMessageFromBytes<Map>(data);

                if (this.OnMapUpdated != null)
                {
                    this.OnMapUpdated(m);
                }
            }
            else if (code == MSGCODE_ROBOT_LOG)
            {
                string text = Helper.GetMessageFromBytes<string>(data);

                if (this.OnLogReceived != null)
                {
                    this.OnLogReceived(text);
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
