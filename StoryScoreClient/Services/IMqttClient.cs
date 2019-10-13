using System;
using System.Threading.Tasks;

namespace StoryScore.Client.Services
{
    internal interface IMqttClient
    {
        event Action<MqttClient.MessageReceived> MessageReceivedEvent;

        Task SendMessageAsync(string topic, string message);
        void Subscribe(string topic);
    }
}