using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using StoryScore.Common;
using StoryScore.Display.Services;

namespace StoryScore.Display.TcpServer
{
    public class ReceiveFile : IDisposable
    {
        public const int Port = 17073;
        private readonly string _fileName;
        private readonly long _fileSize;
        private readonly FileManagerService _fileManager;

        public event Action<FileTransferStatus> FileReceived;
        public event Action<FileTransferStatus> FileTransferStatus;

        public IPAddress PublicIPAddress
        {
            get
            {
                var iphostInfo = Dns.GetHostEntry(Dns.GetHostName());
                return iphostInfo.AddressList.FirstOrDefault(p => !p.IsIPv6LinkLocal) ?? iphostInfo.AddressList[0];
            }
        }

        public ReceiveFile(FileTransferStatus file, FileManagerService fileManager)
        {
            _fileName    = file.Name;
            _fileSize    = file.FileSize;
            _fileManager = fileManager;
        }

        public void Listen()
        {
            var sw            = new Stopwatch();
            var localEndpoint = new IPEndPoint(PublicIPAddress, Port);
            var tcpListener   = new TcpListener(localEndpoint);
            var fileStatus    = new FileTransferStatus { Name = _fileName, FileSize = _fileSize, TransferredBytes = 0 };
            long totalLength  = 0;

            const int StatusUpdate = 10000;

            tcpListener.Start();

            using (var tcpClient = tcpListener.AcceptTcpClient())
            {
                sw.Start();
                using (var nwStream = tcpClient.GetStream())
                {
                    using (var stream = _fileManager.CreateFile(_fileName))
                    {
                        var bytes = new byte[1024];
                        var data = new List<byte>();
                        int length;
                        int counter = 0;

                        while ((length = nwStream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            stream.Write(bytes, 0, length);
                            totalLength += length;

                            if ((++counter) > StatusUpdate) // every MB
                            {
                                counter = 0;
                                fileStatus.TransferredBytes    = totalLength;
                                fileStatus.ElapsedMilliseconds = sw.ElapsedMilliseconds;
                                FileTransferStatus?.Invoke(fileStatus);
                            }
                        }
                    }
                }
            }

            tcpListener.Stop();

            fileStatus.ElapsedMilliseconds = sw.ElapsedMilliseconds;
            fileStatus.TransferComplete    = true;
            fileStatus.TransferredBytes    = totalLength;
            FileReceived?.Invoke(fileStatus);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ReceiveFile()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}