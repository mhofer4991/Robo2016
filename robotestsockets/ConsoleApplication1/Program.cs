using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("192.168.0.189"), 4444);

            try
            {
                client.Connect(ipe);

                Console.WriteLine("connected!");

                NetworkStream stream = client.GetStream();
                StreamWriter writer = new StreamWriter(stream);

                writer.WriteLine("test");
                writer.Flush();
            }
            catch (SocketException)
            {
                Console.WriteLine("not!");
            }

            Console.ReadLine();
        }
    }
}
