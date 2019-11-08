using StoryScore.Common;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace StoryScore.Client.Services
{
    public class TcpClient
    {
        private readonly TcpServer _tcpServer;

        public TcpClient(TcpServer tcpServer)
        {
            _tcpServer = tcpServer;
        }

        public async Task SendFileAsync(string fileName)
        {
            using (var tcpClient = new System.Net.Sockets.TcpClient(_tcpServer.IPAddress, _tcpServer.Port))
            {
                using (NetworkStream networkStream = tcpClient.GetStream())
                {
                    using (var fileStream = File.OpenRead(fileName))
                    {
                        var bytes = new byte[1024];
                        int length;

                        while ((length = await fileStream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                        {
                            await networkStream.WriteAsync(bytes, 0, length);
                        }
                    }

                    await networkStream.FlushAsync();
                }
            }
        }
    }
}
