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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StoryScoreClient.Controls
{
    /// <summary>
    /// Interaction logic for MacthControl.xaml
    /// </summary>
    public partial class MatchControl : UserControl
    {
        public MatchViewModel Model = new MatchViewModel();

        public MatchControl()
        {
            InitializeComponent();
            this.DataContext = Model;
        }

        public void Init(IEnumerable<Team> teams)
        {
            HomeTeamComboBox.ItemsSource = teams;
            AwayTeamComboBox.ItemsSource = teams;
        }

        private void HomeGoalButton_Click(object sender, RoutedEventArgs e)
        {
            Model.HomeScore++;
        }
        private void AwayGoalButton_Click(object sender, RoutedEventArgs e)
        {
            Model.AwayScore++;
        }
    }
}
