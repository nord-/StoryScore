using Newtonsoft.Json;
using StoryScore.Client.Model;
using StoryScore.Common;
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
            var topic = GetTopic(Common.Constants.Mqtt.Update);
            await _mqttClient.SendMessageAsync(topic, JsonConvert.SerializeObject(scoreboard));
        }

        public async Task StartTimerAsync()
        {
            var topic = GetTopic(Common.Constants.Mqtt.Start);
            await _mqttClient.SendMessageAsync(topic, "empty");
        }

        public async Task StopTimerAsync()
        {
            var topic = GetTopic(Common.Constants.Mqtt.Stop);
            await _mqttClient.SendMessageAsync(topic, "empty");
        }

        public async Task StopTimerAsync(TimeSpan offset)
        {
            var topic = GetTopic(Common.Constants.Mqtt.Stop);
            await _mqttClient.SendMessageAsync(topic, JsonConvert.SerializeObject(offset));
        }

        public async Task UpdateGoalAsync(Goal goal)
        {
            var topic = GetTopic(Common.Constants.Mqtt.Goal);
            await _mqttClient.SendMessageAsync(topic, JsonConvert.SerializeObject(goal));
        }
    }
}
