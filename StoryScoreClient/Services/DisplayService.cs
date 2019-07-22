using Newtonsoft.Json;
using StoryScore.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Client.Services
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
            var topic = GetTopic(Events.Update);
            await _mqttClient.SendMessageAsync(topic, JsonConvert.SerializeObject(scoreboard));
        }

        public async Task StartTimerAsync()
        {
            var topic = GetTopic(Events.Start);
            await _mqttClient.SendMessageAsync(topic, "empty");
        }

        public async Task StopTimerAsync()
        {
            var topic = GetTopic(Events.Stop);
            await _mqttClient.SendMessageAsync(topic, "empty");
        }

        public async Task StopTimerAsync(TimeSpan offset)
        {
            var topic = GetTopic(Events.Stop);
            await _mqttClient.SendMessageAsync(topic, JsonConvert.SerializeObject(offset));
        }

        public class Events
        {
            public const string Start = "start";
            public const string Update = "update";
            public const string Stop = "stop";
        }
    }
}
