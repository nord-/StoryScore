using StoryScore.Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace StoryScore.Client
{
    /// <summary>
    /// Interaction logic for LineUp.xaml
    /// </summary>
    public partial class LineUp : Window
    {
        public LineupViewModel ViewModel { get; set; } = new LineupViewModel();


        public LineUp()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void AddHomePlayer_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedHomePlayerPool != null)
            {
                ViewModel.HomeLineUp.Add(ViewModel.SelectedHomePlayerPool);
                ViewModel.HomeTeam.Players.Remove(ViewModel.SelectedHomePlayerPool);
            }
        }

        private void RemoveHomePlayer_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedHomePlayerLineup != null)
            {
                ViewModel.HomeTeam.Players.Add(ViewModel.SelectedHomePlayerLineup);
                ViewModel.HomeLineUp.Remove(ViewModel.SelectedHomePlayerLineup);
            }
        }

        private void AddHomePlayer_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddHomePlayer_Click(sender, null);
        }

        private void RemoveHomePlayer_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            RemoveHomePlayer_Click(sender, null);
        }

        private void AddAwayPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedAwayPlayerPool != null)
            {
                ViewModel.AwayLineUp.Add(ViewModel.SelectedAwayPlayerPool);
                ViewModel.AwayTeam.Players.Remove(ViewModel.SelectedAwayPlayerPool);
            }

        }

        private void RemoveAwayPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedAwayPlayerLineup != null)
            {
                ViewModel.AwayTeam.Players.Add(ViewModel.SelectedAwayPlayerLineup);
                ViewModel.AwayLineUp.Remove(ViewModel.SelectedAwayPlayerLineup);
            }

        }

        private void AddAwayPlayer_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddAwayPlayer_Click(sender, null);
        }

        private void RemoveAwayPlayer_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            RemoveAwayPlayer_Click(sender, null);
        }
    }
}
