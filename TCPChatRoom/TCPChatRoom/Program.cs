using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace TCPClient
{
    class Program
    {

        static string name;
        static int port = 9000;
        static IPAddress ip;
        static Socket sck;
        static Thread rec;


        static string GetLocalIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "127.0.0.1";
        }

        static void RecieveBuffer()
        {
            while (true)
            {
                Thread.Sleep(500);
                byte[] buffer = new byte[300];
                int rece = sck.Receive(buffer, 0, buffer.Length, 0);
                Array.Resize(ref buffer, rece);
                Console.WriteLine(Encoding.Default.GetString(buffer));
            }
        }

        static void Main(string[] args)
        {
            rec = new Thread(RecieveBuffer);
            Console.WriteLine(" Your Ip is " + GetLocalIP());
            Console.WriteLine(" Please enter your Name");
            name = Console.ReadLine();
            ip = IPAddress.Parse(GetLocalIP());
            Console.WriteLine(" Please enter your IP Address to connect: ");
            string prt = Console.ReadLine();
            Console.WriteLine(" You are now connected {0}. \nSay Hello: ", name);

            try
            {
                port = Convert.ToInt32(prt);

            }
            catch
            {
                port = 9000;
            }

            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sck.Connect(new IPEndPoint(ip, port));
            rec.Start();
            byte[] data = Encoding.Default.GetBytes("<" + name + " > is Connected ");
            sck.Send(data, 0, data.Length, 0);

            while (sck.Connected)
            {
                byte[] sdata = Encoding.Default.GetBytes("<" + name + "> " + Console.ReadLine());
                sck.Send(sdata, 0, sdata.Length, 0);
               

            }
        }
    }
}
