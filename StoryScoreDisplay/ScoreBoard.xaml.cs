using Newtonsoft.Json;
using StoryScore.Common;
using StoryScore.Display.CustomControls;
using StoryScore.Display.Models;
using StoryScore.Display.Services;
using System;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace StoryScore.Display
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ScoreBoardWindow : Window
    {
        private readonly Timer _timer                  = new Timer(1000);
        private readonly Timer _homeAndAwayScrollTimer = new Timer(2000);

        private DateTime _startTime          = DateTime.MinValue;
        private TimeSpan _currentElapsedTime = TimeSpan.Zero;
        private TimeSpan _timerOffset        = TimeSpan.Zero;
        private bool     _timerIsRunning     = false;

        private readonly Options     _options = new Options();
        private          Mqtt.Server _mqttServer;

        private readonly Mqtt.Client _mqttClient;

        //private Thread _fileTransferThread;
        private readonly FileManagerService _fileManagerService;

        private          ScoreBoardModel _model;
        private readonly MediaService    _mediaService;
        private readonly AdsService      _adsService;

        public ScoreBoardWindow()
        {
            InitializeComponent();

            _timer.Elapsed                  += Timer_Elapsed;
            _homeAndAwayScrollTimer.Elapsed += HomeAndAwayScrollTimer_Elapsed;


            _model           = new ScoreBoardModel();
            this.DataContext = _model;

            _mqttServer                      =  new Mqtt.Server(_options);
            _mqttClient                      =  new Mqtt.Client(_options);
            _mqttClient.MessageReceivedEvent += MqttClient_MessageReceivedEvent;
            _mqttClient.Subscribe($"{Common.Constants.Topic.Display}/{_options.ClientId}/#"); // subscribe to all updates meant for me!
            Task.Run(async () => await _mqttClient.SendMessageAsync($"{Common.Constants.Topic.Display}/{_options.ClientId}/{Common.Constants.Mqtt.Status}",
                                                                    "online")); // tell the world I'm here!

            _fileManagerService = new FileManagerService(_options, _mqttClient);

            _mediaService              =  MediaService.CreateInstance(this, _options);
            _mediaService.VideoPlaying += MediaServiceOnVideoPlaying;
            _mediaService.VideoStopped += MediaServiceOnVideoStopped;

            _adsService                 =  new AdsService(_options);
            _adsService.AdSourceChanged += AdsService_AdSourceChanged;
        }

        private void AdsService_AdSourceChanged(string adPath)
        {
            var img = LowerPartImage;
            Dispatcher.Invoke(() =>
                              {
                                  img.Visibility = string.IsNullOrWhiteSpace(adPath) ? Visibility.Collapsed : Visibility.Visible;

                                  if (img.Source != null)
                                  {
                                      // fade out old image
                                      var fadeOut = new DoubleAnimation {From = 1, To = 0, Duration = new Duration(TimeSpan.FromMilliseconds(500))};
                                      fadeOut.Completed += (sender, args) =>
                                                           {
                                                               if (!string.IsNullOrWhiteSpace(adPath))
                                                               {
                                                                   // fade in new image
                                                                   var fadeIn = new DoubleAnimation
                                                                                {From = 0, To = 1, Duration = new Duration(TimeSpan.FromMilliseconds(500))};
                                                                   img.Source = new BitmapImage(new Uri(adPath));
                                                                   img.BeginAnimation(OpacityProperty, fadeIn);
                                                               }
                                                               else
                                                               {
                                                                   img.Source = null;
                                                               }
                                                           };
                                      img.BeginAnimation(OpacityProperty, fadeOut);
                                  }
                                  else
                                  {
                                      // fade in new image
                                      img.Opacity = 0;
                                      if (!string.IsNullOrWhiteSpace(adPath))
                                      {
                                          var fadeIn = new DoubleAnimation
                                                       {From = 0, To = 1, Duration = new Duration(TimeSpan.FromMilliseconds(500))};
                                          img.Source = new BitmapImage(new Uri(adPath));
                                          img.BeginAnimation(OpacityProperty, fadeIn);
                                          //_adFilePath = value;
                                          //AdImage.Parent.Dispatcher.Invoke(() => AdImage.BeginAnimation(UIElement.OpacityProperty, fadeIn));
                                      }
                                      else
                                      {
                                          img.Source = null;
                                      }
                                  }
                              });
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

                    Dispatcher.Invoke(DisplayLineup);

                    _homeAndAwayScrollTimer.Start();

                    break;

                case Common.Constants.Mqtt.HideLineup:
                    HideLineup();
                    break;

                case Common.Constants.Mqtt.SendFile:
                    Task.Run(async () => await _fileManagerService.StartReceiveFileAsync(messageAsJson));
                    break;

                case Common.Constants.Mqtt.ReqListFiles:
                    var files = _fileManagerService.GetRemoteFiles();
                    Task.Run(async () => await _mqttClient.SendMessageAsync(_mqttClient.GetTopic(Common.Constants.Mqtt.ListFiles), files));
                    break;

                case Common.Constants.Mqtt.VideoPlaybackAction:
                    var mediamsg = JsonConvert.DeserializeObject<MediaFileAction>(messageAsJson);
                    HandleMediaMessage(mediamsg);
                    break;

                case Common.Constants.Mqtt.ShowAds:
                    _mediaService.StopVideo();
                    HideLineup();
                    _adsService.ShowAds(messageAsJson);
                    break;

                case Common.Constants.Mqtt.HideAds:
                    _adsService.HideAds();
                    break;

                default:
                    Debug.Print("Unknown message");
                    break;
            }
        }

        private void HideLineup()
        {
            _homeAndAwayScrollTimer.Stop();
            Dispatcher.Invoke(() =>
                              {
                                  homeScroll.Visibility =
                                      awayScroll.Visibility = Visibility.Collapsed;
                              });
        }

        private void HandleMediaMessage(MediaFileAction msg)
        {
            switch (msg.Action)
            {
                case MediaFileActionEnum.Play:
                    _mediaService.PlayVideo(msg.FileName, msg.RequestMade);
                    break;
            }
        }

        private async void MediaServiceOnVideoPlaying(string filename, DateTime started, DateTime requested)
        {
            var topic   = _mqttClient.GetTopic(Common.Constants.Mqtt.VideoPlaybackAction);
            var message = new MediaFileAction {Action = MediaFileActionEnum.Noop, FileName = filename, RequestMade = started, OriginalRequest = requested};

            await _mqttClient.SendMessageAsync(topic, message);
        }

        private async void MediaServiceOnVideoStopped(string filename, DateTime requestAt, DateTime requested)
        {
            var topic = _mqttClient.GetTopic(Common.Constants.Mqtt.VideoPlaybackAction);
            var msg   = new MediaFileAction {Action = MediaFileActionEnum.Stop, FileName = filename, RequestMade = requestAt, OriginalRequest = requested};

            await _mqttClient.SendMessageAsync(topic, msg);
        }

        private void DisplayLineup()
        {
            HomeInformationList.Items.Clear();
            foreach (var p in _model.HomePlayers)
            {
                var tb = new TextBlock
                         {
                             FontSize     = 18,
                             Foreground   = new SolidColorBrush(Colors.White),
                             TextTrimming = TextTrimming.CharacterEllipsis,
                             MaxWidth     = HomeInformationList.ActualWidth,
                             Text         = p.NameNumberAndPosition
                         };
                HomeInformationList.Items.Add(tb);
            }

            homeScroll.Visibility = Visibility.Visible;

            AwayInformationList.Items.Clear();
            foreach (var p in _model.AwayPlayers)
            {
                var tb = new TextBlock
                         {
                             FontSize     = 18,
                             Foreground   = new SolidColorBrush(Colors.White),
                             TextTrimming = TextTrimming.CharacterEllipsis,
                             MaxWidth     = AwayInformationList.ActualWidth,
                             Text         = p.NameNumberAndPosition
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
            var heartbeat = JsonConvert.SerializeObject(new Heartbeat {DisplayId = _options.ClientId, Matchclock = _currentElapsedTime});
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
                _timerOffset     = offset.Value;
                _model.GameClock = _timerOffset;
            }
            else
                _timerOffset = _currentElapsedTime;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {}

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
            const int OffsetAmount    = 50;
            const int NumberOfSeconds = 2;

            var offset  = (double)homeScroll.GetValue(SmoothScrollViewer.MyOffsetProperty);
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

            offset  = (double)awayScroll.GetValue(SmoothScrollViewer.MyOffsetProperty);
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