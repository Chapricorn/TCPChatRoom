using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

namespace TCPServer
{
    class Program
    {
        static Socket listenerSocket;
        static Socket acc;
        static int port = 9000;
        static IPAddress ip;
        static Thread receive;
        static string name;
        static Queue queue = new Queue();


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
                int rece = acc.Receive(buffer, 0, buffer.Length, 0);
                Array.Resize(ref buffer, rece);
                Console.WriteLine(Encoding.Default.GetString(buffer));
                queue.Enqueue(Encoding.Default.GetString(buffer));

                //DisplayQueueItems();
                // TODO: how do I print the queue
            }


        }

        /// <summary>
        /// Print queue items
        /// </summary>
        public void DisplayQueueItems()
        {
            Console.WriteLine("Number of elements in the Queue: {0}", queue.Count);

            while (queue.Count > 0)
                Console.WriteLine(queue.Dequeue());

            Console.WriteLine("Number of elements in the Queue: {0}", queue.Count);
            Console.ReadLine();
        }


        /// <summary>
        /// Main Method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Queue queue = new Queue();


            receive = new Thread(RecieveBuffer);
            Console.WriteLine(" Your Local Ip is " + GetLocalIP());
            Console.WriteLine(" Please enter your name ");
            name = Console.ReadLine();
            Console.WriteLine(" Please enter your IP Address to connect: ");
            string prt = Console.ReadLine();
            Console.WriteLine(" You are now connected {0}. \n Wait for a User to Connect. ", name);

            try
            {
                port = Convert.ToInt32(prt);
            }
            catch
            {
                port = 9000;
            }

            ip = IPAddress.Parse(GetLocalIP());

            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenerSocket.Bind(new IPEndPoint(ip, port));
            listenerSocket.Listen(0);
            acc = listenerSocket.Accept();
            receive.Start();
            while (true)
            {
                byte[] sdata = Encoding.Default.GetBytes("<" + name + ">" + Console.ReadLine());
                acc.Send(sdata, 0, sdata.Length, 0);
            
            }


        }
    }
}
