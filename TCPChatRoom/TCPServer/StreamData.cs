using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace StreamData
{
    public class StreamData : BinaryWriter
    {
        public List<string> AllData;
        public string userID;
        public TypeData typeData;

        public StreamData(TypeData type, string senderId)
            : base()
        {
            AllData = new List<string>();
            userID = userID;
            typeData = type;
        }
        public byte[] ToBytes()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, this);
            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();
            return bytes;
        }
        public StreamData(byte[] packetBytes)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(packetBytes);

            StreamData newStream = (StreamData)binaryFormatter.Deserialize(memoryStream);
            memoryStream.Close();
            AllData = newStream.AllData;
            userID = newStream.userID;
            typeData = newStream.typeData;
        }
        public byte[] ToByteArray(object packet)
        {
            using (var stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, packet);
                return stream.ToArray();
            }
        }
        public static string GetIPAddress() 
        {
            IPAddress[] ip = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ipAddress in ip)
            {
                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ipAddress.ToString();
                }
            }
            return GetIPAddress();
        }
    }

    public enum TypeData
    {
        StreamNotification,
        stream,
    }
}

