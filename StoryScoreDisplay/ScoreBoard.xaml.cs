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

            this.DataContext = new ScoreBoardModel();

            _mqttServer = new Mqtt.Server(_options);
            _mqttClient = new Mqtt.Client(_options);
            _mqttClient.MessageReceivedEvent += MqttClient_MessageReceivedEvent;
            _mqttClient.Subscribe("display/+/updates"); // subscribe to all updates!
            Task.Run(async () => await _mqttClient.SendMessageAsync($"display/{_options.ClientId}/status", "online"));  // tell the world I'm here!

            _model = new ScoreBoardModel();

        }

        private void MqttClient_MessageReceivedEvent(MQTTnet.MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timeSinceStartTime = DateTime.Now - _startTime;
            timeSinceStartTime = new TimeSpan(timeSinceStartTime.Hours,
                                              timeSinceStartTime.Minutes,
                                              timeSinceStartTime.Seconds);

            // The current elapsed time is the time since the start button
            // was clicked, plus the total time elapsed since the last reset
            _currentElapsedTime = timeSinceStartTime;

            // TODO: update displayed time (take into offset into account)
        }

        public void StartTimer()
        {
            if (!_timerIsRunning)
            {
                _startTime = DateTime.Now;
                _timer.Start();
                _timerIsRunning = true;
            }
        }

        public void StopTimer(TimeSpan? offset)
        {
            _timer.Stop();
            _timerIsRunning = false;

            if (offset.HasValue)
                _timerOffset = offset.Value;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
