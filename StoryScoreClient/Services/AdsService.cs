using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using Newtonsoft.Json;

namespace StoryScore.Client.Services
{
    public class AdsService
    {
        private readonly IMqttClient _mqttClient;
        private readonly Options     _options;

        public AdsService(Options options)
        {
            _mqttClient = new MqttClient(options);
            _options    = options;

            _mqttClient.MessageReceivedEvent += MqttClientOnMessageReceivedEvent;
            //_mqttClient.Subscribe(Common.Constants.Topic.Sync);
            //_mqttClient.Subscribe($"{Common.Constants.Topic.Display}/+/{Common.Constants.Mqtt.ReceiveFile}");
            //_mqttClient.Subscribe("#");
        }

        private void MqttClientOnMessageReceivedEvent(MqttApplicationMessageReceivedEventArgs obj)
        {
            throw new NotImplementedException();
        }

        private string GetTopic(string @event)
        {
            return $"{Common.Constants.Topic.Display}/{_options.ReceiverClientId}/{@event}";
        }

        public async Task ShowAdsAsync()
        {
            var topic = GetTopic(Common.Constants.Mqtt.ShowAds);
            var ads = JsonConvert.SerializeObject(new[]
                                                  {
                                                      "Digital_reklam_Arena_Alvhogsborg.jpg", "Digital_reklam_Bravida.jpg", "Digital_reklam_Contekton.jpg",
                                                      "Digital_reklam_Dalek.jpg", "Digital_reklam_Eidar.jpg", "Digital_reklam_Folkets_hus.jpg", "Digital_reklam_GKN.jpg",
                                                      "Digital_reklam_Handelsbanken.jpg", "Digital_reklam_IQR.jpg", "Digital_reklam_lilleskogs.jpg",
                                                      "Digital_reklam_Mobelmastarna.jpg", "Digital_reklam_Nevs.jpg", "Digital_reklam_Nolato.jpg",
                                                      "Digital_reklam_Serneke.jpg", "Digital_reklam_Severinssons.jpg", "Digital_reklam_Sjuntorp.jpg",
                                                      "Digital_reklam_Spec.jpg", "Digital_reklam_Stavdal.jpg", "Digital_reklam_ThnStad.jpg", "Digital_reklam_tiab.jpg",
                                                      "Digital_reklam_vbgstadsbud.jpg"
                                                  });
            await _mqttClient.SendMessageAsync(topic, ads);
        }

        public async Task HideAdsAsync()
        {
            var topic = GetTopic(Common.Constants.Mqtt.HideAds);
            await _mqttClient.SendMessageAsync(topic, "empty");
        }
    }
}