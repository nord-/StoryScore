using Microsoft.Win32;
using StoryScore.Client.Model;
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
            BuildMediaButtons();

            PageModel.PropertyChanged += PageModel_PropertyChanged;
            DataContext = PageModel;
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
                    BuildMediaButtons();

                    Properties.Settings.Default["MediaFolderPath"] = PageModel.FolderPath;
                    break;
            }
        }

        private void BuildMediaButtons()
        {
            const double ButtonHeight = 100.0;
            const double ButtonWidth = 100.0;

            //var basePanel = ButtonsStackPanel;
            //var basePanel = MediaFileItems;
            //basePanel.ItemsSource = PageModel.MediaFolders.First().Files;


            //basePanel.Children.Clear();

            //var ctxMenu = this.FindResource("SyncContextMenu") as ContextMenu;

            //foreach (var f in PageModel.MediaFolders)
            //{
            //    var tb = new TextBlock { Text = f.Name, Background = f.BackgroundColor, Foreground = f.ForegroundColor, TextAlignment = TextAlignment.Center, FontWeight = FontWeights.Bold };
            //    basePanel.Children.Add(tb);

            //    var wp = new WrapPanel();
            //    foreach (var mediafile in f.Files)
            //    {
            //        var button = new Button { Height = ButtonHeight, Width = ButtonWidth };
            //        button.Content = new Viewbox
            //        {
            //            Stretch = Stretch.Uniform,
            //            StretchDirection = StretchDirection.DownOnly,
            //            Child =
            //            new TextBlock { Text = mediafile.Name, TextWrapping = TextWrapping.Wrap, Width = ButtonWidth, TextAlignment = TextAlignment.Center }
            //        };
            //        // TODO: click handler

            //        button.ContextMenu = ctxMenu;

            //        var canvas = new Canvas { Width = ButtonWidth, Height = ButtonHeight, Margin = new Thickness(2, 4, 2, 4) };
            //        canvas.Children.Add(button);

            //        var fontFamily = App.Current.FindResource("FontAwesomeSolid") as FontFamily;
            //        var brokenLink = new TextBlock { Text = "\uf127", Foreground = new SolidColorBrush(Colors.DarkGray), FontFamily = fontFamily,
            //                                         Visibility = (mediafile.Synced ? Visibility.Collapsed : Visibility.Visible) };
            //        brokenLink.SetValue(Canvas.RightProperty, 5.0);
            //        brokenLink.SetValue(Canvas.BottomProperty, 5.0);
            //        canvas.Children.Add(brokenLink);
            //        var solidLink = new TextBlock { Text = "\uf0c1", FontFamily = fontFamily,
            //                                        Visibility = (mediafile.Synced ? Visibility.Visible : Visibility.Collapsed) };
            //        solidLink.SetValue(Canvas.RightProperty, 5.0);
            //        solidLink.SetValue(Canvas.BottomProperty, 5.0);
            //        canvas.Children.Add(solidLink);

            //        wp.Children.Add(canvas);
            //    }
            //    basePanel.Children.Add(wp);
            //}
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

        private void SyncContextMenu_Click(object sender, RoutedEventArgs e)
        {
            Debug.Print((sender as Control).Name);
            MessageBox.Show($"{((((((e.Source as MenuItem).Parent as ContextMenu).Parent as System.Windows.Controls.Primitives.Popup).PlacementTarget as Button).Content as Viewbox).Child as TextBlock).Text}");
        }
    }
}
