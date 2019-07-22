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

        private bool _matchStarted = false;

        public event Action<object, EventArgs> ScoreChanged;
        public event Action<object, EventArgs> MatchStarted;
        public event Action<object, EventArgs> ClockStarted;
        public event Action<object, ClockStoppedEventArgs> ClockStopped;

        public class ClockStoppedEventArgs : EventArgs
        {
            public TimeSpan TimerOffset { get; set; }
            public bool OffsetSet { get; set; } = false;
        }

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

        private void StartMatchButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_matchStarted)
            {
                HomeTeamComboBox.IsEnabled =
                    AwayTeamComboBox.IsEnabled = false;
                StartClockButton.IsEnabled =
                    ChangeTimeButton.IsEnabled =
                    HomeGoalButton.IsEnabled =
                    AwayGoalButton.IsEnabled = true;

                StartMatchButton.Content = "Stop match";
                StartMatchButton.Background = Brushes.Red;
                _matchStarted = true;

                OnMatchStarted(e);
            }
            else
            {
                HomeTeamComboBox.IsEnabled =
                    AwayTeamComboBox.IsEnabled = true;
                StartClockButton.IsEnabled =
                    StopClockButton.IsEnabled =
                    ChangeTimeButton.IsEnabled =
                    HomeGoalButton.IsEnabled =
                    AwayGoalButton.IsEnabled = false;
                OnClockStopped(new ClockStoppedEventArgs { OffsetSet = true, TimerOffset = TimeSpan.Zero });

                StartMatchButton.Content = "Start match";
                StartMatchButton.Background = Brushes.Green;
                _matchStarted = false;
            }
        }

        private void StartClockButton_Click(object sender, RoutedEventArgs e)
        {
            StopClockButton.IsEnabled = true;
            StartClockButton.IsEnabled = false;
            OnClockStarted(e);
        }

        private void StopClockButton_Click(object sender, RoutedEventArgs e)
        {
            StopClockButton.IsEnabled = false;
            StartClockButton.IsEnabled = true;
            OnClockStopped(new ClockStoppedEventArgs { OffsetSet = false });
        }

        private void ChangeTimeButton_Click(object sender, RoutedEventArgs e)
        {
            var inputTime = new InputTimeWindow();
            // TODO: need to know what timer is...

            var args = new ClockStoppedEventArgs();

            inputTime.Owner = Window.GetWindow(this);
            var result = inputTime.ShowDialog() ?? false;
            if (result)
            {
                args.TimerOffset = inputTime.OffsetTime;
                args.OffsetSet = true;

                OnClockStopped(args);
            }
        }

        private void HomeGoalScorerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HomeUndoGoalButton_Click(object sender, RoutedEventArgs e)
        {
            Model.HomeScore--;
            OnScoreChange(e);
        }

        private void AwayGoalScorerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AwayUndoGoalButton_Click(object sender, RoutedEventArgs e)
        {
            Model.AwayScore--;
            OnScoreChange(e);
        }

        #region Event handlers
        protected virtual void OnScoreChange(EventArgs e)
        {
            ScoreChanged?.Invoke(this, e);
        }

        protected virtual void OnMatchStarted(EventArgs e)
        {
            MatchStarted?.Invoke(this, e);
        }

        protected virtual void OnClockStarted(EventArgs e)
        {
            ClockStarted?.Invoke(this, e);
        }

        protected virtual void OnClockStopped(ClockStoppedEventArgs e)
        {
            ClockStopped?.Invoke(this, e);
        }
        #endregion
    }
}
