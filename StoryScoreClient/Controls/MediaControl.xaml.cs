using Microsoft.Win32;
using StoryScore.Client.Model;
using System;
using System.Collections.Generic;
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

            PopulateMedia();
            BuildMediaButtons();

            PageModel.PropertyChanged += PageModel_PropertyChanged;

            DataContext = PageModel;
        }

        private void PageModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(PageModel.FolderPath):
                    PopulateMedia();
                    BuildMediaButtons();
                    break;
            }
        }

        private void BuildMediaButtons()
        {
            var basePanel = ButtonsStackPanel;
            basePanel.Children.Clear();

            foreach (var f in PageModel.MediaFolders)
            {
                var tb = new TextBlock { Text = f.Name, Background = f.BackgroundColor, Foreground = f.ForegroundColor, TextAlignment = TextAlignment.Center };
                basePanel.Children.Add(tb);

                var wp = new WrapPanel { Orientation = Orientation.Horizontal };
                foreach (var mediafile in f.Files)
                {
                    var button = new Button { Height = 100, Width = 100, Margin = new Thickness(2, 4, 2, 4) };
                    button.Content = new Viewbox
                    {
                        Stretch = Stretch.Uniform,
                        StretchDirection = StretchDirection.DownOnly,
                        Child =
                        new TextBlock { Text = mediafile.Name, TextWrapping = TextWrapping.Wrap, Width = 50, TextAlignment = TextAlignment.Center }
                    };
                    // TODO: click handler
                    wp.Children.Add(button);
                }
                basePanel.Children.Add(wp);
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
    }
}
