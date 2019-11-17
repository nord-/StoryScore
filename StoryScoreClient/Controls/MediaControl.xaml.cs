using StoryScore.Client.Model;
using StoryScore.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Timer = System.Timers.Timer;

namespace StoryScore.Client.Controls
{
    /// <summary>
    /// Interaction logic for MediaControl.xaml
    /// </summary>
    public partial class MediaControl : UserControl
    {
        int _fadeCounter;
        private readonly Timer _fadeOutTimer = new Timer(150);

        public MediaControlViewModel PageModel { get; set; } = new MediaControlViewModel();
        public long Delay { get; set; }

        public event Action<string> StartVideoPlayback;

        public MediaControl()
        {
            InitializeComponent();

            GetFolderPath();
            PopulateMedia();

            _fadeOutTimer.Elapsed += FadeOutTimer_Elapsed;

            PageModel.Player = MediaPlayer;
            PageModel.PropertyChanged += PageModel_PropertyChanged;
            DataContext = PageModel;

            PageModel.Player.MediaEnded += (sender, args) =>
                                           {
                                               MediaPlayer.Close();
                                               PageModel.SelectedMediaFile = null;
                                           };
        }

        public void Init(FileTransferService fileTransferService)
        {
            PageModel.FileTransferService = fileTransferService;
        }

        private void GetFolderPath()
        {
            PageModel.FolderPath = (string)Properties.Settings.Default["MediaFolderPath"];
            if (string.IsNullOrWhiteSpace(PageModel.FolderPath))
            {
                PageModel.FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }

        private void PageModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(PageModel.FolderPath):
                    PopulateMedia();

                    Properties.Settings.Default["MediaFolderPath"] = PageModel.FolderPath;
                    break;

                case nameof(PageModel.SelectedMediaFile):
                    if (!string.IsNullOrEmpty(PageModel.SelectedMediaFile?.Name ?? "") && (PageModel.SelectedMediaFile?.Synced ?? false))
                    {
                        StartVideoPlayback?.Invoke(PageModel.SelectedMediaFile.File.Name);
                    }
                    else if (!string.IsNullOrEmpty(PageModel.SelectedMediaFile?.Name ?? ""))
                    {
                        // play locally
                        Play();
                    }
                    break;
            }
        }


        private void PopulateMedia()
        {
            var mediaFolders = new List<MediaFolder>();

            var folders = new System.IO.DirectoryInfo(PageModel.FolderPath).GetDirectories();
            foreach (var f in folders)
            {
                var files = f.GetFiles()
                             .Where(p => !p.Attributes.HasFlag(System.IO.FileAttributes.Hidden))
                             .Select(p => new MediaFile
                             {
                                 Name = p.Name,
                                 Extension = System.IO.Path.GetExtension(p.FullName),
                                 Path = p.FullName,
                                 File = p
                             }).ToArray();

                var folder = new MediaFolder(f.FullName, files)
                {
                    Name   = f.Name,
                    Folder = f
                };

                mediaFolders.Add(folder);
            }

            PageModel.MediaFolders = new System.Collections.ObjectModel.ObservableCollection<MediaFolder>(mediaFolders);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _fadeOutTimer.Start();
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Play();
        }

        public void Play()
        {
            Dispatcher.Invoke(() =>
                              {
                                  MediaPlayer.Volume = 1.0;
                                  MediaPlayer.Play();
                              });
        }

        private void FadeOutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _fadeCounter++;
            var volume = (100.0 - _fadeCounter * 10.0) / 100.0;
            Dispatcher.Invoke(() => MediaPlayer.Volume = volume);
            if (_fadeCounter != 10) return;

            _fadeCounter = 0;
            _fadeOutTimer.Stop();
            Dispatcher.Invoke(() =>
            {
                MediaPlayer.Stop();
                PageModel.SelectedMediaFile = null;
            });
        }
    }
}
