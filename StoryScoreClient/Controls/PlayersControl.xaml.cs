using AutoMapper;
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
    /// Interaction logic for PlayersControl.xaml
    /// </summary>
    public partial class PlayersControl : UserControl
    {
        private IEnumerable<PlayerViewModel> _players;

        public event Action<PlayersControl> Close;

        public IEnumerable<PlayerViewModel> Players
        {
            get => _players;
            set
            {
                _players = value;
                PlayersListBox.ItemsSource = _players;
            }
        }

        public PlayersControl()
        {
            InitializeComponent();
        }

        private void AddPlayerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemovePlayerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlayersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PlayersListBox.SelectedIndex >= 0)
            {
                EditPlayerControl.DataContext = PlayersListBox.SelectedItem;
                EditPlayerControl.Visibility = Visibility.Visible;
                RemovePlayerButton.IsEnabled = true;
            }
            else
            {
                EditPlayerControl.DataContext = null;
                EditPlayerControl.Visibility = Visibility.Hidden;
                RemovePlayerButton.IsEnabled = false;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            OnClose();
        }

        protected void OnClose()
        {
            PlayersListBox.SelectedIndex = -1;
            PlayersListBox.ItemsSource = null;
            Close?.Invoke(this);
        }
    }
}
