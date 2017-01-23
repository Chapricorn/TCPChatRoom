using System;
using StreamData;
using System.Net.Sockets;
using System.Threading;


namespace Server
{
    public class UserData
    {
        public Socket userSocket;
        public Thread UserThread;
        public string ID;

        public UserData()
        {
            ID = Guid.NewGuid().ToString();
            UserThread = new Thread(Server.UserData);
            UserThread.Start(userSocket);
            SendStreamNotification();
        }

        public UserData(Socket userSocket)
        {
            this.userSocket = userSocket;
            ID = Guid.NewGuid().ToString();
            UserThread = new Thread(Server.UserData);
            UserThread.Start(userSocket);
            SendStreamNotification();
        }

        public void SendStreamNotification()
        {
            StreamData.StreamData packet = new StreamData.StreamData(TypeData.StreamNotification, "server");
            packet.AllData.Add(ID);
            userSocket.Send(packet.ToBytes());
        }
    }
}
