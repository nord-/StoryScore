using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScoreClient.Services
{
    public class DisplayService : IDisposable
    {
        private const string ClientID = "Control";
        private readonly IManagedMqttClient _mqttClient;

        public DisplayService(Options options)
        {
            // Create a new MQTT client.
            var opt = new ManagedMqttClientOptionsBuilder()
                            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                            .WithClientOptions(new MqttClientOptionsBuilder()
                                .WithClientId($"{ClientID}_{options.ClientId}")
                                .WithTcpServer(options.DisplayEndpoint, options.Port)
                                .Build()
                            )
                            .Build();

            _mqttClient = new MqttFactory().CreateManagedMqttClient();
            //_mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("#").Build()).Wait();
            _mqttClient.StartAsync(opt).Wait();

            _mqttClient.UseApplicationMessageReceivedHandler(ReceiveMessage);
        }

        private void ReceiveMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            Debug.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
            Debug.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
            Debug.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            Debug.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
            Debug.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
            Debug.WriteLine("");

            //MessageReceivedEvent?.Invoke(e);
        }

        public void Subscribe(string topic)
        {
            _mqttClient.UseConnectedHandler(async e =>
            {
                Debug.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to a topic
                await _mqttClient.SubscribeAsync(new TopicFilterBuilder()
                                 .WithTopic(topic)
                                 .Build());

                Debug.WriteLine($"### SUBSCRIBED TO TOPIC {topic} ###");
            });
        }

        public async Task SendMessageAsync(string topic, string message)
        {

            var msg = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .Build();

            Debug.WriteLine("### SENDING MESSAGE ###");
            Debug.WriteLine($"{topic}: {message}");

            await _mqttClient.PublishAsync(msg);

            Debug.WriteLine("### MESSAGE SENT ###");
        }

        public void Dispose()
        {
            _mqttClient.StopAsync().Wait();
            _mqttClient.Dispose();
        }

        public class Events
        {
            public const string Start = "start";
            public const string Update = "update";
            public const string Stop = "stop";
        }
    }
}
