using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using StoryScore.Common;

namespace StoryScore.Display.Services
{
    public sealed class MediaService
    {
        private readonly ScoreBoardWindow _window;
        private readonly Options          _options;
        private static   MediaService     _instance;

        private readonly MediaElement        _videoPlayer;
        private          Image               _fsImage;
        private          Image               _adImage;
        private          MediaFileActionEnum _videoPlayerState;
        private          string              _filename;
        private          DateTime            _requestedAt;

        public event Action<string, DateTime, DateTime> VideoPlaying;
        public event Action<string, DateTime, DateTime> VideoStopped;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static MediaService() {}

        private MediaService(ScoreBoardWindow window, Options options)
        {
            _window                 =  window;
            _options                =  options;
            _videoPlayer            =  _window.FullScreenVideo;
            _videoPlayer.MediaEnded += VideoPlayerOnMediaEnded;
            _fsImage                =  _window.FullScreenImage;
            _adImage                =  _window.LowerPartImage;
        }

        private void VideoPlayerOnMediaEnded(object sender, RoutedEventArgs e)
        {
            Debug.Print($"{_filename} finished playing...");
            VideoStopped?.Invoke(_filename, DateTime.Now, _requestedAt);
            _window.Dispatcher.Invoke(() =>
                                      {
                                          _videoPlayer.Visibility = Visibility.Collapsed;
                                          _videoPlayer.Close();
                                          ((Canvas)_videoPlayer.Parent).Background = new SolidColorBrush(Colors.Transparent);
                                      });
        }

        public void PlayVideo(string filename, DateTime requestAt)
        {
            _filename    = filename;
            _requestedAt = requestAt;
            CloseVideo();
            _videoPlayerState = MediaFileActionEnum.Play;
            try
            {
                VideoSource = new Uri(Path.Combine(_options.FileStorePath, filename), UriKind.Absolute);
                PlayVideo();
            }
            catch (Exception e)
            {
                throw;
            }

            //_videoPlayer.Stop();
        }

        public void StopVideo()
        {
            _filename         = "";
            _requestedAt      = DateTime.MinValue;
            _videoPlayerState = MediaFileActionEnum.Stop;
            CloseVideo();
        }

        public Uri VideoSource
        {
            get => _videoPlayer.Source;
            private set
            {
                _window.Dispatcher.Invoke(() =>
                                          {
                                              ((Canvas)_videoPlayer.Parent).Background = new SolidColorBrush(Colors.Black);
                                              _videoPlayer.Visibility = Visibility.Visible;
                                              _videoPlayer.Source     = value;
                                          });
            }
        }

        private void PlayVideo()
        {
            VideoPlaying?.Invoke(_filename, DateTime.Now, _requestedAt);
            _window.Dispatcher.Invoke(() => _videoPlayer.Play());
        }

        private void CloseVideo()
        {
            _window.Dispatcher.Invoke(() =>
                                      {
                                          _videoPlayer.Close();
                                          _videoPlayer.Visibility = Visibility.Collapsed;
                                          ((Canvas)_videoPlayer.Parent).Background = new SolidColorBrush(Colors.Transparent);
                                      });
        }

        #region Instance

        public static MediaService CreateInstance(ScoreBoardWindow window, Options options)
        {
            _instance = new MediaService(window, options);
            return Instance;
        }

        public static MediaService Instance => _instance;

        #endregion
    }
}