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

        public event Action<object, EventArgs> ScoreChanged;
        public event Action<object, EventArgs> MatchStarted;

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
            OnScoreChange(e);
        }
        private void AwayGoalButton_Click(object sender, RoutedEventArgs e)
        {
            Model.AwayScore++;
            OnScoreChange(e);
        }

        protected virtual void OnScoreChange(EventArgs e)
        {
            ScoreChanged?.Invoke(this, e);
        }

        protected virtual void OnMatchStarted(EventArgs e)
        {
            MatchStarted?.Invoke(this, e);
        }

        private void StartMatchButton_Click(object sender, RoutedEventArgs e)
        {
            HomeTeamComboBox.IsEnabled =
                AwayTeamComboBox.IsEnabled = false;

            OnMatchStarted(e);
        }
    }
}
