using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace StoryScore.Display.TcpServer
{
    public class ReceiveFile
    {
        public const int Port = 17073;
        private readonly string _fileName;
        private readonly Options _options;

        public IPAddress PublicIPAddress
        {
            get
            {
                var iphostInfo = Dns.GetHostEntry(Dns.GetHostName());
                return iphostInfo.AddressList[0];
            }
        }

        public ReceiveFile(string fileName, Options options)
        {
            _fileName = fileName;
            _options = options;
        }

        public void Listen()
        {
            var localEndpoint = new IPEndPoint(PublicIPAddress, Port);
            var tcpListener = new TcpListener(localEndpoint);

            tcpListener.Start();

            using (var tcpClient = tcpListener.AcceptTcpClient())
            {
                using (var nwStream = tcpClient.GetStream())
                {
                    using (var stream = new FileStream(Path.Combine(_options.FileStorePath, _fileName), FileMode.Create))
                    {
                        var bytes = new byte[1024];
                        var data = new List<byte>();
                        int length;

                        while ((length = nwStream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            stream.Write(bytes, 0, length);
                        }
                    }
                }
            }

            tcpListener.Stop();
        }
    }
}