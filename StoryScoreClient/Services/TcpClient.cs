using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Client.Services
{
    public class TcpClient
    {
        private readonly string _fileName;

        public TcpClient(string fileName)
        {
            _fileName = fileName;
        }

        public void SendFile(Common.TcpServer tcpServer)
        {
            using (var tcpClient = new System.Net.Sockets.TcpClient(tcpServer.IPAddress, tcpServer.Port))
            {
                using (NetworkStream networkStream = tcpClient.GetStream())
                {
                    using (var fileStream = File.OpenRead(_fileName))
                    {
                        var bytes = new byte[1024];
                        int length;

                        while ((length = fileStream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            networkStream.Write(bytes, 0, length);
                        }
                    }
                    //var dataToSend = File.ReadAllBytes(_fileName);

                    //networkStream.Write(dataToSend, 0, dataToSend.Length);
                    networkStream.Flush();
                }
            }
        }
    }
}
