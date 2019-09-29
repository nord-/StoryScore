using AutoMapper;
using StoryScore.Client.Data;
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
        private List<PlayerViewModel> _players;
        private Team _team;
        private readonly IPlayerRepository _repository;

        public event Action<PlayersControl> Close;

        public List<PlayerViewModel> Players
        {
            get => _players;
            set
            {
                _players = value;
                PlayersListBox.ItemsSource = _players;
            }
        }

        public Team Team { get => _team; internal set => _team = value; }

        public PlayersControl()
        {
            InitializeComponent();
            _repository = new PlayerRepository();

            EditPlayerControl.Init(_repository);
            EditPlayerControl.Save += EditPlayerControl_Save;
            EditPlayerControl.Cancel += EditPlayerControl_Cancel;
        }

        private void EditPlayerControl_Cancel(EditPlayerControl target, PlayerViewModel player)
        {
            var index = -1;
            if (player == null)
            {
                // pressed cancel on new player
                var p = _players.FirstOrDefault(pl => pl.Id == 0);
                if (p != null) _players.Remove(p);
            }

            foreach (var p in _players)
            {
                if (p.Id == player.Id)
                {
                    index = _players.IndexOf(p);
                    break;
                }
            }

            if (index > -1)
            {
                _players[index] = player;
                PlayersListBox.ItemsSource = _players;
            }

            PlayersListBox.SelectedIndex = -1;
        }

        private void EditPlayerControl_Save(EditPlayerControl target, PlayerViewModel player)
        {
            PlayersListBox.SelectedIndex = -1;
        }

        private void AddPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            var newItem = new PlayerViewModel { Team = Team };
            _players.Add(newItem);
            PlayersListBox.ItemsSource = _players;
            PlayersListBox.SelectedItem = newItem;
        }

        private void RemovePlayerButton_Click(object sender, RoutedEventArgs e)
        {
            _repository.RemovePlayer(Mapper.Map<Player>(PlayersListBox.SelectedItem));

            _players.Remove((PlayerViewModel)PlayersListBox.SelectedItem);
            PlayersListBox.ItemsSource = _players;
        }

        private void PlayersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PlayersListBox.SelectedIndex >= 0)
            {
                //EditPlayerControl.DataContext = PlayersListBox.SelectedItem;
                EditPlayerControl.ViewModel = (PlayerViewModel)PlayersListBox.SelectedItem;
                EditPlayerControl.Visibility = Visibility.Visible;
                RemovePlayerButton.IsEnabled = true;
            }
            else
            {
                //EditPlayerControl.DataContext = null;
                EditPlayerControl.ViewModel = null;
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
