using AutoMapper;
using StoryScore.Client.Data;
using StoryScore.Client.Model;
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
            _repository.SavePlayer(Mapper.Map<Player>(DataContext));
            Save?.Invoke(this, (PlayerViewModel)DataContext);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = Mapper.Map<PlayerViewModel>(_repository.GetPlayer(((PlayerViewModel)DataContext).Id));
            Cancel?.Invoke(this, (PlayerViewModel)DataContext);
        }
    }
}
