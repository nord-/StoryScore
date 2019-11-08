using Microsoft.Win32;
using StoryScore.Client.Model;
using StoryScore.Client.Services;
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

namespace StoryScore.Client.Controls
{
    /// <summary>
    /// Interaction logic for MediaControl.xaml
    /// </summary>
    public partial class MediaControl : UserControl
    {
        int fadeCounter = 0;
        Timer FadeOutTimer = new Timer(150);

        public MediaControlViewModel PageModel { get; set; } = new MediaControlViewModel();

        public MediaControl()
        {
            InitializeComponent();

            GetFolderPath();
            PopulateMedia();

            FadeOutTimer.Elapsed += FadeOutTimer_Elapsed;

            PageModel.Player = MediaPlayer;
            PageModel.PropertyChanged += PageModel_PropertyChanged;
            DataContext = PageModel;
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
                    MediaPlayer.Volume = 1.0;
                    MediaPlayer.Play();
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

        private void StopButton_Click(object sender, RoutedEventArgs e) {
            FadeOutTimer.Start();
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Volume = 1.0;
            MediaPlayer.Play();
        }

        private void FadeOutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            fadeCounter++;
            var volume = (100.0 - fadeCounter * 10.0) / 100.0;
            Dispatcher.Invoke(() => MediaPlayer.Volume = volume);
            if (fadeCounter == 10)
            {
                fadeCounter = 0;
                FadeOutTimer.Stop();
                Dispatcher.Invoke(() =>
                {
                    MediaPlayer.Stop();
                    PageModel.SelectedMediaFile = null;
                });
            }
        }

        //private void SyncContextMenu_Click(object sender, RoutedEventArgs e)
        //{
        //    Debug.Print((sender as Control).Name);
        //    MessageBox.Show($"{((((((e.Source as MenuItem).Parent as ContextMenu).Parent as System.Windows.Controls.Primitives.Popup).PlacementTarget as Button).Content as Viewbox).Child as TextBlock).Text}");
        //}
    }
}
