using Newtonsoft.Json;
using StoryScore.Common;
using StoryScore.Display.CustomControls;
using StoryScore.Display.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(1000);
        private readonly System.Timers.Timer _homeAndAwayScrollTimer = new System.Timers.Timer(2000);

        private DateTime _startTime = DateTime.MinValue;
        private TimeSpan _currentElapsedTime = TimeSpan.Zero;
        private TimeSpan _timerOffset = TimeSpan.Zero;
        private bool _timerIsRunning = false;

        private Options _options = new Options();
        private Mqtt.Server _mqttServer;
        private Mqtt.Client _mqttClient;
        private Thread _fileTransferThread;

        private ScoreBoardModel _model;

        public ScoreBoardWindow()
        {
            InitializeComponent();

            _timer.Elapsed += Timer_Elapsed;
            _homeAndAwayScrollTimer.Elapsed += HomeAndAwayScrollTimer_Elapsed;


            _model = new ScoreBoardModel();
            this.DataContext = _model;

            _mqttServer = new Mqtt.Server(_options);
            _mqttClient = new Mqtt.Client(_options);
            _mqttClient.MessageReceivedEvent += MqttClient_MessageReceivedEvent;
            _mqttClient.Subscribe($"{Common.Constants.Topic.Display}/{_options.ClientId}/#"); // subscribe to all updates meant for me!
            Task.Run(async () => await _mqttClient.SendMessageAsync($"{Common.Constants.Topic.Display}/{_options.ClientId}/{Common.Constants.Mqtt.Status}", "online"));  // tell the world I'm here!
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
                    var msg = JsonConvert.DeserializeObject<LineupModel>(messageAsJson);
                    _model.HomePlayers = msg.Home;
                    _model.AwayPlayers = msg.Away;

                    Dispatcher.Invoke(() => DisplayLineup());

                    _homeAndAwayScrollTimer.Start();

                    break;

                case Common.Constants.Mqtt.HideLineup:
                    _homeAndAwayScrollTimer.Stop();
                    Dispatcher.Invoke(() =>
                    {
                        homeScroll.Visibility =
                            awayScroll.Visibility = Visibility.Collapsed;
                    });
                    break;

                case Common.Constants.Mqtt.SendFile:
                    StartReceiveFile(messageAsJson);
                    break;

                default:
                    Debug.Print("Unknown message");
                    break;
            }
        }

        private void StartReceiveFile(string messageAsJson)
        {
            var fileInfo = JsonConvert.DeserializeObject<FileTransferStatus>(messageAsJson);
            var fileService = new TcpServer.ReceiveFile(fileInfo, _options);
            fileService.FileReceived += FileService_FileReceived;
            fileService.FileTransferStatus += FileService_FileTransferStatus;

            var message = new Common.TcpServer
            {
                IPAddress = fileService.PublicIPAddress.ToString(),
                Port = TcpServer.ReceiveFile.Port
            };

            _fileTransferThread = new Thread(new ThreadStart(fileService.Listen));
            _fileTransferThread.Start();

            message.Timestamp = DateTime.Now;

            Task.Run(async () => await _mqttClient.SendMessageAsync(_mqttClient.GetTopic(Common.Constants.Mqtt.ReceiveFile), message));
        }

        private async void FileService_FileTransferStatus(FileTransferStatus txStatus)
        {
            await _mqttClient.SendMessageAsync(_mqttClient.GetTopic(Common.Constants.Mqtt.TransferStatus), txStatus);
        }

        private async void FileService_FileReceived(FileTransferStatus txStatus)
        {
            Debug.Print($"File received: {txStatus.Name} took {txStatus.ElapsedMilliseconds} ms.");
            await _mqttClient.SendMessageAsync(_mqttClient.GetTopic(Common.Constants.Mqtt.TransferStatus), txStatus);

            if (_fileTransferThread.IsAlive)
            {
                _fileTransferThread.Abort();
                _fileTransferThread = null;
            }
        }

        private void DisplayLineup()
        {
            HomeInformationList.Items.Clear();
            foreach (var p in _model.HomePlayers)
            {
                var tb = new TextBlock
                {
                    FontSize = 18,
                    Foreground = new SolidColorBrush(Colors.White),
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    MaxWidth = HomeInformationList.ActualWidth,
                    Text = p.NameNumberAndPosition
                };
                HomeInformationList.Items.Add(tb);
            }
            homeScroll.Visibility = Visibility.Visible;

            AwayInformationList.Items.Clear();
            foreach (var p in _model.AwayPlayers)
            {
                var tb = new TextBlock
                {
                    FontSize = 18,
                    Foreground = new SolidColorBrush(Colors.White),
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    MaxWidth = AwayInformationList.ActualWidth,
                    Text = p.NameNumberAndPosition
                };
                AwayInformationList.Items.Add(tb);
            }
            awayScroll.Visibility = Visibility.Visible;

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
                foreach (TextBlock tb in HomeInformationList.Items.Cast<TextBlock>())
                    tb.MaxWidth = homeScroll.ActualWidth - (homeScroll.Padding.Left + homeScroll.Padding.Right);
                foreach (TextBlock tb in AwayInformationList.Items.Cast<TextBlock>())
                    tb.MaxWidth = awayScroll.ActualWidth - (awayScroll.Padding.Left + awayScroll.Padding.Right);

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
            DoubleAnimation goDown;
            if (offset > (content.RenderSize.Height - homeScroll.RenderSize.Height))
            {
                goDown = new DoubleAnimation(offset, 0, new Duration(TimeSpan.FromSeconds(NumberOfSeconds)));
            }
            else
            {
                goDown = new DoubleAnimation(offset, offset + OffsetAmount, new Duration(TimeSpan.FromSeconds(NumberOfSeconds)));
            }
            homeScroll.BeginAnimation(SmoothScrollViewer.MyOffsetProperty, goDown);

            offset = (double)awayScroll.GetValue(SmoothScrollViewer.MyOffsetProperty);
            content = awayScroll.Content as ListBox;
            Debug.Print($"{awayScroll.ContentVerticalOffset}\t{offset}\t{content.RenderSize.Height}");
            if (offset > (content.RenderSize.Height - homeScroll.RenderSize.Height))
            {
                goDown = new DoubleAnimation(offset, 0, new Duration(TimeSpan.FromSeconds(NumberOfSeconds)));
            }
            else
            {
                goDown = new DoubleAnimation(offset, offset + OffsetAmount, new Duration(TimeSpan.FromSeconds(NumberOfSeconds)));
            }
            awayScroll.BeginAnimation(SmoothScrollViewer.MyOffsetProperty, goDown);
        }
    }
}
