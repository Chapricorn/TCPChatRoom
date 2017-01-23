using StreamData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Server
    {
        static Socket listenerSocket;
        static List<UserData> Users;

        public static Queue<string> messageQueue = new Queue<string>();
        public static Dictionary<TcpClient, string> clientDictionary = new Dictionary<TcpClient, string>();

        public static void Main(string[] args)
        {
            Console.WriteLine(" Connecting to Ip Address: " + StreamData.StreamData.GetIPAddress());

            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Users = new List<UserData>();

            IPEndPoint IP_End = new IPEndPoint(IPAddress.Parse(StreamData.StreamData.GetIPAddress()), 4242);
            listenerSocket.Bind(IP_End);

            Thread listenThread = new Thread(ListenThread);
            listenThread.Start();

            Console.WriteLine(" Connected! Waiting for Users to Connect \nListening IP: " + StreamData.StreamData.GetIPAddress());
        }
        public static void addToUserList()
        {
            TcpListener myList;
            myList = new TcpListener(IPAddress.Any, 4242);
            while (true)
            {
                TcpClient user = myList.AcceptTcpClient();
                clientDictionary.Add(user, " name ");
            }
        }

        static void ListenThread()
        {
            while (true)
            {
                listenerSocket.Listen(0);
                Users.Add(new UserData(listenerSocket.Accept()));
                Console.WriteLine(" A User has joined!");
            }
        }

        public static void UserData(object user_socket)
        {
            Socket userSocket = (Socket)user_socket;

            byte[] buffer;
            int readBytes;

            for (;;)
            {
                try
                {
                    buffer = new byte[userSocket.SendBufferSize];
                    readBytes = userSocket.Receive(buffer);

                    if (readBytes > 0)
                    {
                        StreamData.StreamData packet = new StreamData.StreamData(buffer);
                        DataManager(packet);
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine(" A User has left.");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
        }

        public static void DataManager(StreamData.StreamData packet)
        {
            switch (packet.typeData)
            {
                case TypeData.stream:
                    foreach (UserData client in Users)
                    {
                        client.userSocket.Send(packet.ToBytes());
                    }
                    break;
            }
        }
    }
}
