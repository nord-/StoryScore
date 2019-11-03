using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace StoryScore.Display.TcpServer
{
    public class ReceiveFile
    {
        public const int Port = 17073;
        private readonly string _fileName;
        private readonly Options _options;

        public event Action<ReceiveFileInfo> FileReceived;

        public IPAddress PublicIPAddress
        {
            get
            {
                var iphostInfo = Dns.GetHostEntry(Dns.GetHostName());
                return iphostInfo.AddressList.FirstOrDefault(p => !p.IsIPv6LinkLocal) ?? iphostInfo.AddressList[0];                
            }
        }

        public ReceiveFile(string fileName, Options options)
        {
            _fileName = fileName;
            _options = options;
        }

        public void Listen()
        {
            var sw            = new Stopwatch();
            var file          = Path.Combine(_options.FileStorePath, _fileName);
            var localEndpoint = new IPEndPoint(PublicIPAddress, Port);
            var tcpListener   = new TcpListener(localEndpoint);

            tcpListener.Start();

            using (var tcpClient = tcpListener.AcceptTcpClient())
            {
                sw.Start();
                using (var nwStream = tcpClient.GetStream())
                {
                    using (var stream = new FileStream(file, FileMode.Create))
                    {
                        var bytes = new byte[1024];
                        var data = new List<byte>();
                        int length;
                        long totalLength = 0;

                        while ((length = nwStream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            stream.Write(bytes, 0, length);
                            totalLength += length;
                            //if (totalLength % 1024 == 0) Debug.Print($"{totalLength/(1024^2)} MiB written");
                        }
                    }
                }
            }

            tcpListener.Stop();

            var receivedFile = new ReceiveFileInfo { FileName = file, ElapsedMilliseconds = sw.ElapsedMilliseconds };
            FileReceived?.Invoke(receivedFile);
        }
    }

    public class ReceiveFileInfo
    {
        public string FileName { get; set; }
        public long ElapsedMilliseconds { get; set; }
    }
}