using System;
using System.Collections.Generic;
using StreamData;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    class Client
    {
        public static Socket mySocket;
        public static string name;
        public static string ID;

        static void Main(string[] args)
        {
            Console.WriteLine(" Please Enter your Name: ");
            name = Console.ReadLine();

            Console.Clear();
            Console.WriteLine(" Please Enter your IP address to Connect:");
            string IP = Console.ReadLine();

            mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint IP_end = new IPEndPoint(IPAddress.Parse(IP), 4242);

            try
            {
                mySocket.Connect(IP_end);
            }
            catch
            {
                Console.WriteLine(" Connection failed. \n");
            }

            Thread thread = new Thread(UserData);
            thread.Start();

            for (;;)
            {
                Console.Write("");
                string input = Console.ReadLine();

                StreamData.StreamData streamPacket = new StreamData.StreamData(TypeData.stream, ID);
                streamPacket.AllData.Add(name);
                streamPacket.AllData.Add(input);
                mySocket.Send(streamPacket.ToBytes());
            }
        }

        static void UserData()
        {
            byte[] buffer;
            int readBytes;

            for (;;)
            {
                try
                {
                    buffer = new byte[mySocket.SendBufferSize];
                    readBytes = mySocket.Receive(buffer);

                    if (readBytes > 0)
                    {
                        DataManager(new StreamData.StreamData(buffer));
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine(" The server has Shut Down! ");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }

        }
        static void DataManager(StreamData.StreamData packet)
        {
            switch (packet.typeData)
            {
                case TypeData.StreamNotification:
                    Console.WriteLine(" You are now connected {0}. ", name);
                    ID = packet.AllData[0];
                    break;
                case TypeData.stream:
                    Console.WriteLine(packet.AllData[0] + ": " + packet.AllData[1]);
                    break;
            }
        }
    }
}