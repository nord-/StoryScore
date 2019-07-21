using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StoryScoreDisplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ScoreBoardWindow : Window
    {
        private Timer _timer;

        private DateTime _startTime = DateTime.MinValue;
        private TimeSpan _currentElapsedTime = TimeSpan.Zero;
        private TimeSpan _timerOffset = TimeSpan.Zero;
        private bool _timerIsRunning = false;

        private Options _options = new Options();
        private Mqtt.Server _mqttServer;
        private Mqtt.Client _mqttClient;

        private ScoreBoardModel _model;

        public ScoreBoardWindow()
        {
            InitializeComponent();

            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += Timer_Elapsed;

            _model = new ScoreBoardModel();
            this.DataContext = _model;

            _mqttServer = new Mqtt.Server(_options);
            _mqttClient = new Mqtt.Client(_options);
            _mqttClient.MessageReceivedEvent += MqttClient_MessageReceivedEvent;
            _mqttClient.Subscribe($"display/{_options.ClientId}/#"); // subscribe to all updates meant for me!
            Task.Run(async () => await _mqttClient.SendMessageAsync($"display/{_options.ClientId}/status", "online"));  // tell the world I'm here!
        }

        private void MqttClient_MessageReceivedEvent(MQTTnet.MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            var messageAsJson = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);

            var topic = eventArgs.ApplicationMessage.Topic.Split('/').Last();
            switch (topic)
            {
                case "start":
                    StartTimer();
                    break;

                case "update":
                    _model = JsonConvert.DeserializeObject<ScoreBoardModel>(messageAsJson);
                    Dispatcher.Invoke(() => DataContext = _model);
                    break;

                case "stop":
                    if (!string.IsNullOrWhiteSpace(messageAsJson) && messageAsJson != "empty")
                    {
                        var offset = JsonConvert.DeserializeObject<TimeSpan>(messageAsJson);
                        StopTimer(offset);
                    }
                    else
                    {
                        StopTimer();
                    }
                    break;

                //case "pause":
                //    break;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timeSinceStartTime = (DateTime.Now - _startTime) + _timerOffset;
            timeSinceStartTime = new TimeSpan(timeSinceStartTime.Hours,
                                              timeSinceStartTime.Minutes,
                                              timeSinceStartTime.Seconds);

            // The current elapsed time is the time since the start button
            // was clicked, plus the total time elapsed since the last reset
            _currentElapsedTime = timeSinceStartTime;

            // update displayed time (take offset into account)
            _model.GameClock = _currentElapsedTime;
        }

        private void StartTimer()
        {
            if (!_timerIsRunning)
            {
                _startTime = DateTime.Now;
                _timer.Start();
                _timerIsRunning = true;
            }
        }

        private void StopTimer(TimeSpan? offset = null)
        {
            _timer.Stop();
            _timerIsRunning = false;

            if (offset.HasValue)
                _timerOffset = offset.Value;
            else
                _timerOffset = _currentElapsedTime;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
