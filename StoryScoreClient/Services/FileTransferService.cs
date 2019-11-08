using StoryScore.Common;
using System;
using System.Threading.Tasks;

namespace StoryScore.Client.Services
{
    public sealed class FileTransferService : IDisposable
    {
        static IMqttClient _mqttClient;
        private readonly Options _options;
        private string _filename = "";

        public event Action<FileTransferStatus> TransferStatus;

        public FileTransferService(Options options)
        {
            _options = options;
            _mqttClient = new MqttClient(_options, "FileTransfer");
            _mqttClient.MessageReceivedEvent += MqttClient_MessageReceivedEvent;
            _mqttClient.Subscribe(GetTopic("+"));
        }

        private async void MqttClient_MessageReceivedEvent(MQTTnet.MqttApplicationMessageReceivedEventArgs msg)
        {
            var topicType = MqttClient.GetLastTopicPart(msg);

            switch (topicType)
            {
                case Common.Constants.Mqtt.SendFile:
                    // don't care, this is the display's responsibility to take care of
                    break;

                case Common.Constants.Mqtt.ReceiveFile:
                    if (string.IsNullOrEmpty(_filename))
                        // not this object's file
                        return;

                    var tcpServerInfo = MqttClient.TranslatePayload<TcpServer>(msg);
                    await SendFileOverTcpAsync(tcpServerInfo, _filename);
                    break;

                case Common.Constants.Mqtt.TransferStatus:
                    // get progress
                    var ft = MqttClient.TranslatePayload<FileTransferStatus>(msg);
                    TransferStatus?.Invoke(ft);
                    break;
            }
        }

        public async Task SendFileAsync(string filename)
        {
            _filename = filename;

            var topic = GetTopic(Common.Constants.Mqtt.SendFile);
            var message = filename.Substring(filename.LastIndexOf('\\') + 1);
            await _mqttClient.SendMessageAsync(topic, message);
        }

        private static async Task SendFileOverTcpAsync(TcpServer tcpServerInfo, string filename)
        {
            var client = new TcpClient(tcpServerInfo);
            await client.SendFileAsync(filename);
        }

        private string GetTopic(string @event)
        {
            return $"{Common.Constants.Topic.Display}/{_options.ReceiverClientId}/{@event}";
        }

        public void Dispose()
        {
            _mqttClient = null;
        }
    }
}
