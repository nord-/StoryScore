using StoryScoreClient.Model;
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
using System.Windows.Shapes;

namespace StoryScoreClient
{
    /// <summary>
    /// Interaction logic for GoalInputWindow.xaml
    /// </summary>
    public partial class GoalInputWindow : Window
    {
        public int MatchTime { get; set; }
        public Player Player { get; set; }

        public IEnumerable<Player> Players { get; set; }

        public GoalInputWindow(IEnumerable<Player> players)
        {
            InitializeComponent();
            Players = players;
            PlayerComboBox.ItemsSource = Players;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            MatchTime = int.Parse(EventTimeTextBox.Text);
            Player = (Player)PlayerComboBox.SelectedItem;

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            PlayerComboBox.Items.Refresh();
        }
    }
}
