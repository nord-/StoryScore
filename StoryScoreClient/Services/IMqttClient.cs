using MQTTnet;
using System;
using System.Threading.Tasks;

namespace StoryScore.Client.Services
{
    public interface IMqttClient
    {
        event Action<MqttApplicationMessageReceivedEventArgs> MessageReceivedEvent;

        Task SendMessageAsync(string topic, string message);
        Task SendMessageAsync(string topic, object message);
        void Subscribe(string topic);
    }
}