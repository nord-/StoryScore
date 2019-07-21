using Newtonsoft.Json;
using StoryScoreClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScoreClient.Services
{

    public class DisplayService
    {
        private readonly MqttClient _mqttClient;
        private readonly Options _options;

        public DisplayService(MqttClient mqttClient, Options options)
        {
            _mqttClient = mqttClient;
            _options = options;
        }

        private string GetTopic(string @event)
        {
            return $"display/{_options.ReceiverClientId}/{@event}";
        }

        public async Task UpdateAsync(Scoreboard scoreboard)
        {
            var topic = GetTopic(MqttClient.Events.Update);
            await _mqttClient.SendMessageAsync(topic, JsonConvert.SerializeObject(scoreboard));
        }

        public async Task StartTimer()
        {
            var topic = GetTopic(MqttClient.Events.Start);
            await _mqttClient.SendMessageAsync(topic, string.Empty);
        }

        public async Task StopTimer()
        {
            var topic = GetTopic(MqttClient.Events.Stop);
            await _mqttClient.SendMessageAsync(topic, string.Empty);
        }

        public async Task StopTimerAsync(TimeSpan offset)
        {
            var topic = GetTopic(MqttClient.Events.Stop);
            await _mqttClient.SendMessageAsync(topic, JsonConvert.SerializeObject(offset));
        }
    }
}
