using MQTTnet;
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

    public class DisplayService : IDisplayService
    {
        private readonly IMqttClient _mqttClient;
        private readonly Options _options;

        public event Action<Heartbeat> MatchClockTick;

        public DisplayService(IMqttClient mqttClient, Options options)
        {
            _mqttClient = mqttClient;
            _options = options;

            _mqttClient.MessageReceivedEvent += MqttClient_MessageReceivedEvent;
            _mqttClient.Subscribe(Common.Constants.Topic.Sync);
            //_mqttClient.Subscribe($"{Common.Constants.Topic.Display}/+/{Common.Constants.Mqtt.ReceiveFile}");
            //_mqttClient.Subscribe("#");
        }

        private void MqttClient_MessageReceivedEvent(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            var heartbeat = MqttClient.TranslatePayload<Heartbeat>(eventArgs);
            MatchClockTick?.Invoke(heartbeat);

            //if (eventArgs.ApplicationMessage.Topic == Common.Constants.Topic.Sync)
            //{
            //var messageAsJson = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
            //var heartbeat = JsonConvert.DeserializeObject<Heartbeat>(messageAsJson);
            //MatchClockTick?.Invoke(heartbeat);
            //}
            //else if (eventArgs.ApplicationMessage.Topic.Contains(Common.Constants.Mqtt.ReceiveFile))
            //{
            //}
        }

        private string GetTopic(string @event)
        {
            return $"{Common.Constants.Topic.Display}/{_options.ReceiverClientId}/{@event}";
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

        public async Task SendLineupAsync(IEnumerable<PlayerViewModel> homeLineUp, IEnumerable<PlayerViewModel> awayLineUp) {
            var topic = GetTopic(Common.Constants.Mqtt.LineUp);
            var message = new { home = homeLineUp, away = awayLineUp };

            await _mqttClient.SendMessageAsync(topic, JsonConvert.SerializeObject(message));
        }

        public async Task HideLineup()
        {
            var topic = GetTopic(Common.Constants.Mqtt.HideLineup);
            await _mqttClient.SendMessageAsync(topic, "empty");
        }
    }
}
