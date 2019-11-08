using MQTTnet;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://github.com/chkr1011/MQTTnet/wiki/Server

namespace StoryScore.Display.Mqtt
{
    public sealed class Server : IDisposable
    {
        private readonly IMqttServer _mqttServer;

        public Server(Options options)
        {
            // Configure MQTT server.
            var optionsBuilder = new MqttServerOptionsBuilder()
                //.WithConnectionBacklog(100)
                .WithDefaultEndpointPort(options.Port);

            _mqttServer = new MqttFactory().CreateMqttServer();
            _mqttServer.StartAsync(optionsBuilder.Build()).Wait();
        }

        public void Dispose()
        {
            _mqttServer.StopAsync().Wait();
        }
    }
}
