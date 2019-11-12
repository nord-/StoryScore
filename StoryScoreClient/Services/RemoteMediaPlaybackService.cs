using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using StoryScore.Common;

namespace StoryScore.Client.Services
{
    public class RemoteMediaPlaybackService
    {
        private readonly Options _options;
        static IMqttClient       _mqttClient;
        private string _filename;
        private DateTime _startedRequest;

        public RemoteMediaPlaybackService(Options options)
        {
            _options = options;
            _mqttClient                      =  new MqttClient(_options, $"RemoteMediaPlayback{_options.ClientId}");
            _mqttClient.MessageReceivedEvent += MqttClient_MessageReceivedEvent;
            _mqttClient.Subscribe(Topic);
        }

        private void MqttClient_MessageReceivedEvent(MqttApplicationMessageReceivedEventArgs obj)
        {
            var msg = MqttClient.TranslatePayload<MediaFileAction>(obj);
            switch (msg.Action)
            {
                case MediaFileActionEnum.Noop:
                    // ack on start
                    if (msg.FileName == _filename)
                        Debug.Print($"NOOP {_filename}\t{msg.RequestMade}\t{(msg.RequestMade-_startedRequest).TotalMilliseconds} ms.\t{(DateTime.Now - _startedRequest).TotalMilliseconds} ms. total");
                    break;

                case MediaFileActionEnum.Stop:
                    // media file stopped
                    break;

                case MediaFileActionEnum.Play:
                    if (msg.FileName == _filename)
                        Debug.Print($"PLAY {_filename}\t{msg.RequestMade}\t{(msg.RequestMade-_startedRequest).TotalMilliseconds} ms.\t{(DateTime.Now - _startedRequest).TotalMilliseconds} ms. total");
                    break;

            }
        }

        public async Task PlayVideoAsync(string filename)
        {
            _filename = filename;
            _startedRequest = DateTime.Now;
            var msg = new MediaFileAction { Action = MediaFileActionEnum.Play, FileName = filename, RequestMade = _startedRequest };

            Debug.Print($"Start play\t{_filename}\t{_startedRequest}");

            await _mqttClient.SendMessageAsync(Topic, msg);
        }

        private string Topic => $"{Common.Constants.Topic.Display}/{_options.ReceiverClientId}/{Common.Constants.Mqtt.VideoPlaybackAction}";
    }
}
