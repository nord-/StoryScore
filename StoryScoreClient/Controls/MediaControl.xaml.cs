using Microsoft.Win32;
using StoryScore.Client.Model;
using StoryScore.Client.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public MediaControlViewModel PageModel { get; set; } = new MediaControlViewModel();

        public MediaControl()
        {
            InitializeComponent();

            GetFolderPath();
            PopulateMedia();

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
            }
        }


        private void PopulateMedia()
        {
            var mediaFolders = new List<MediaFolder>();

            var folders = new System.IO.DirectoryInfo(PageModel.FolderPath).GetDirectories();
            foreach (var f in folders)
            {
                var files = f.GetFiles();
                var folder = new MediaFolder
                {
                    Name   = f.Name,
                    Path   = f.FullName,
                    Folder = f,
                    Files  = files.Select(p => new MediaFile
                    {
                        Name      = p.Name,
                        Extension = System.IO.Path.GetExtension(p.FullName),
                        Path      = p.FullName,
                        File      = p
                    }).ToArray()
                };

                mediaFolders.Add(folder);
            }

            PageModel.MediaFolders = new System.Collections.ObjectModel.ObservableCollection<MediaFolder>(mediaFolders);
        }

        //private void SyncContextMenu_Click(object sender, RoutedEventArgs e)
        //{
        //    Debug.Print((sender as Control).Name);
        //    MessageBox.Show($"{((((((e.Source as MenuItem).Parent as ContextMenu).Parent as System.Windows.Controls.Primitives.Popup).PlacementTarget as Button).Content as Viewbox).Child as TextBlock).Text}");
        //}
    }
}
