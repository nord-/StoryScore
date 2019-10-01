using AutoMapper;
using StoryScore.Client.Model;
using StoryScore.Data.Domain;
using StoryScore.Data.Repository;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StoryScore.Client.Controls
{
    /// <summary>
    /// Interaction logic for EditPlayerControl.xaml
    /// </summary>
    public partial class EditPlayerControl : UserControl
    {
        private IPlayerRepository _repository;

        public event Action<EditPlayerControl, PlayerViewModel> Save;
        public event Action<EditPlayerControl, PlayerViewModel> Cancel;

        internal PlayerViewModel ViewModel
        {
            get => (PlayerViewModel)DataContext;
            set => DataContext = value;
        }
        private Player Player
        {
            get
            {
                return Mapper.Map<Player>(ViewModel);
            }

            set
            {
                ViewModel = Mapper.Map<PlayerViewModel>(value);
            }
        }

        public EditPlayerControl()
        {
            InitializeComponent();
        }

        internal void Init(IPlayerRepository repository)
        {
            _repository = repository;
        }


        private void SavePlayerButton_Click(object sender, RoutedEventArgs e)
        {
            Player = _repository.SavePlayer(Player);
            Save?.Invoke(this, ViewModel);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Player.Id != 0)
                Player = _repository.GetPlayer(Player.Id);

            //DataContext = Mapper.Map<PlayerViewModel>(_repository.GetPlayer(((PlayerViewModel)DataContext).Id));
            Cancel?.Invoke(this, ViewModel);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var self = sender as UserControl;
            if (self.Visibility == Visibility.Visible)
            {
                PlayerNumberTextBox.Focus();
            }
        }
    }
}
