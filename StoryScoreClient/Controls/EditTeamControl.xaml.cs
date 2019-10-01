using Microsoft.Win32;
using StoryScore.Client.Model;
using StoryScore.Data.Domain;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StoryScore.Client.Controls
{
    /// <summary>
    /// Interaction logic for EditTeamControl.xaml
    /// </summary>
    public partial class EditTeamControl : UserControl
    {
        public event Action<object, EventArgs> SaveClicked;
        public event Action<object, EventArgs> CancelClicked;
        public event Action<EditTeamControl> ViewPlayersClicked;

        public TeamViewModel ViewModel { get => (TeamViewModel)DataContext; set => DataContext = value; }

        public EditTeamControl()
        {
            InitializeComponent();
        }

        private void Save_Clicked(object sender, RoutedEventArgs e)
        {
            OnSaveClicked(e);
        }

        private void Cancel_Clicked(object sender, RoutedEventArgs e)
        {
            OnCancelClicked(e);
        }

        protected virtual void OnSaveClicked(EventArgs e)
        {
            SaveClicked?.Invoke(this, e);
        }
        protected virtual void OnCancelClicked(EventArgs e)
        {
            CancelClicked?.Invoke(this, e);
        }

        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.png;*.jpeg;*.jpg|All files|*.*";
            if (openFileDialog.ShowDialog() == true)
                ViewModel.LogoPath = openFileDialog.FileName;
        }

        private void PlayersButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Id == 0)
            {
                MessageBox.Show("Please save the team first.");
                return;
            }
            OnViewPlayersClicked();
        }

        protected virtual void OnViewPlayersClicked()
        {
            ViewPlayersClicked?.Invoke(this);
        }
    }
}
