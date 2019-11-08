using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Display.Mqtt
{
    public sealed class Client : IDisposable
    {
        private const string ClientID = "Display";
        private readonly IManagedMqttClient _mqttClient;
        private readonly Options _options;

        public delegate void MessageReceived(MqttApplicationMessageReceivedEventArgs eventArgs);
        public event MessageReceived MessageReceivedEvent;

        public Client(Options options)
        {
            // Create a new MQTT client.
            var opt = new ManagedMqttClientOptionsBuilder()
                            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                            .WithClientOptions(new MqttClientOptionsBuilder()
                                .WithClientId($"{ClientID}_{options.ClientId}")
                                .WithTcpServer("127.0.0.1", options.Port)
                                .Build()
                            )
                            .Build();

            _mqttClient = new MqttFactory().CreateManagedMqttClient();
            //_mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("#").Build()).Wait();
            _mqttClient.StartAsync(opt).Wait();


            _mqttClient.UseApplicationMessageReceivedHandler(ReceiveMessage);
            _options = options;
        }

        private async Task ReceiveMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            Debug.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
            Debug.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
            Debug.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            Debug.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
            Debug.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
            Debug.WriteLine("");

            MessageReceivedEvent?.Invoke(e);
        }

        public void Subscribe(string topic)
        {
            //_mqttClient.UseConnectedHandler(e =>
            //{
            //    _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(topic).Build()).Wait();
            //});

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

        public async Task SendMessageAsync(string topic, object message)
        {
            var messageAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            await SendMessageAsync(topic, messageAsJson);
        }

        public async Task SendMessageAsync(string topic, string message)
        {
            var msg = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .Build();

            await _mqttClient.PublishAsync(msg);
        }

        public string GetTopic(string @event)
        {
            return $"{Common.Constants.Topic.Display}/{_options.ClientId}/{@event}";
        }

        public void Dispose()
        {
            _mqttClient.StopAsync().Wait();
            _mqttClient.Dispose();
        }
    }

}
