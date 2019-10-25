using Newtonsoft.Json;
using StoryScore.Common;
using StoryScore.Display.CustomControls;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace StoryScore.Display
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ScoreBoardWindow : Window
    {
        private readonly Timer _timer = new Timer(1000);
        private readonly Timer _homeAndAwayScrollTimer = new Timer(2000);

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

            _timer.Elapsed += Timer_Elapsed;
            _homeAndAwayScrollTimer.Elapsed += HomeAndAwayScrollTimer_Elapsed;
            _homeAndAwayScrollTimer.Start();


            _model = new ScoreBoardModel();
            this.DataContext = _model;

            _mqttServer = new Mqtt.Server(_options);
            _mqttClient = new Mqtt.Client(_options);
            _mqttClient.MessageReceivedEvent += MqttClient_MessageReceivedEvent;
            _mqttClient.Subscribe($"{Common.Constants.Topic.Display}/{_options.ClientId}/#"); // subscribe to all updates meant for me!
            Task.Run(async () => await _mqttClient.SendMessageAsync($"{Common.Constants.Topic.Display}/{_options.ClientId}/{Common.Constants.Mqtt.Status}", "online"));  // tell the world I'm here!

            for (int i = 1; i < 50; i++)
            {
                var tb = new TextBlock
                {
                    Text = $"{i}: Item {i}",
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 16
                };
                HomeInformationList.Items.Add(tb);
            }

            for (int i = 1; i < 100; i++)
            {
                var tb = new TextBlock
                {
                    Text = $"{i}: Item {i}",
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 16
                };
                AwayInformationList.Items.Add(tb);
            }

        }


        private void MqttClient_MessageReceivedEvent(MQTTnet.MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            var messageAsJson = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);

            var topic = eventArgs.ApplicationMessage.Topic.Split('/').Last();
            switch (topic)
            {
                case Common.Constants.Mqtt.Start:
                    StartTimer();
                    break;

                case Common.Constants.Mqtt.Update:
                    _model = JsonConvert.DeserializeObject<ScoreBoardModel>(messageAsJson);
                    Dispatcher.Invoke(() => DataContext = _model);
                    break;

                case Common.Constants.Mqtt.Stop:
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

                case Common.Constants.Mqtt.Goal:
                    var goal = JsonConvert.DeserializeObject<Goal>(messageAsJson);
                    // TODO: show scorer presentation
                    // TODO: play scorer goal video
                    if (goal.IsHomeTeam)
                    {
                        _model.HomeScore = goal.Score;
                    }
                    else
                    {
                        _model.AwayScore = goal.Score;
                    }
                    break;

                case Common.Constants.Mqtt.LineUp:
                    Debug.Print($"{messageAsJson}\n");
                    break;

                default:
                    Debug.Print("Unknown message");
                    break;
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

            // send heartbeat
            var heartbeat = JsonConvert.SerializeObject(new Heartbeat { DisplayId = _options.ClientId, Matchclock = _currentElapsedTime });
            Task.Run(async () => await _mqttClient.SendMessageAsync(Common.Constants.Topic.Sync, heartbeat));
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
            {
                _timerOffset = offset.Value;
                _model.GameClock = _timerOffset;
            }
            else
                _timerOffset = _currentElapsedTime;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void HomeAndAwayScrollTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ScrollHomeAndAwayInformation();
            });
        }

        private void ScrollHomeAndAwayInformation()
        {
            const int OffsetAmount = 50;
            const int NumberOfSeconds = 2;

            var offset = (double)homeScroll.GetValue(SmoothScrollViewer.MyOffsetProperty);
            var content = homeScroll.Content as ListBox;
            Debug.Print($"{homeScroll.ContentVerticalOffset}\t{offset}\t{content.RenderSize.Height}");
            if (offset > (content.RenderSize.Height - homeScroll.RenderSize.Height))
                offset = 0;

            DoubleAnimation goDown = new DoubleAnimation(
                offset,
                offset + OffsetAmount,
                new Duration(TimeSpan.FromSeconds(NumberOfSeconds)));
            homeScroll.BeginAnimation(SmoothScrollViewer.MyOffsetProperty, goDown);

            offset = (double)awayScroll.GetValue(SmoothScrollViewer.MyOffsetProperty);
            content = awayScroll.Content as ListBox;
            Debug.Print($"{awayScroll.ContentVerticalOffset}\t{offset}\t{content.RenderSize.Height}");
            if (offset > (content.RenderSize.Height - homeScroll.RenderSize.Height))
                offset = 0;

            goDown = new DoubleAnimation(
                offset,
                offset + OffsetAmount,
                new Duration(TimeSpan.FromSeconds(NumberOfSeconds)));
            awayScroll.BeginAnimation(SmoothScrollViewer.MyOffsetProperty, goDown);
        }
    }
}
