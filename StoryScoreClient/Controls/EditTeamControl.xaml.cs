using Microsoft.Win32;
using StoryScore.Client.Model;
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
            Team self = (Team)this.DataContext;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.png;*.jpeg;*.jpg|All files|*.*";
            if (openFileDialog.ShowDialog() == true)
                ((Team)this.DataContext).LogoPath = openFileDialog.FileName;
        }

        private void PlayersButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
