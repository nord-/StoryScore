using StoryScore.Client.Model;
using StoryScoreData.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for MacthControl.xaml
    /// </summary>
    public partial class MatchControl : UserControl
    {
        public MatchViewModel Model = new MatchViewModel();

        private bool _matchStarted = false;

        public event Action<object, GoalEventArgs> ScoreChanged;
        public event Action<object, EventArgs> MatchStarted;
        public event Action<object, EventArgs> ClockStarted;
        public event Action<object, ClockStoppedEventArgs> ClockStopped;

        public class ClockStoppedEventArgs : EventArgs
        {
            public TimeSpan TimerOffset { get; set; }
            public bool OffsetSet { get; set; } = false;
        }

        public class GoalEventArgs : EventArgs
        {
            public int Score { get; set; }
            public int MatchTime { get; set; }
            public Player Player { get; set; }
            public bool IsHomeGoal { get; set; }
            public bool IsCorrection { get; set; }
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
            OnScoreChange((int)Math.Ceiling(Model.Matchclock.TotalMinutes), null, true);
        }
        private void AwayGoalButton_Click(object sender, RoutedEventArgs e)
        {
            Model.AwayScore++;
            OnScoreChange((int)Math.Ceiling(Model.Matchclock.TotalMinutes), null, false);
        }

        private void StartMatchButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_matchStarted)
            {
                HomeTeamComboBox.IsEnabled =
                    AwayTeamComboBox.IsEnabled = false;
                StartClockButton.IsEnabled =
                    ChangeTimeButton.IsEnabled =
                    HomeGoalScorerButton.IsEnabled =
                    AwayGoalScorerButton.IsEnabled =
                    HomeUndoGoalButton.IsEnabled =
                    AwayUndoGoalButton.IsEnabled =
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
                    HomeGoalScorerButton.IsEnabled =
                    AwayGoalScorerButton.IsEnabled =
                    HomeUndoGoalButton.IsEnabled =
                    AwayUndoGoalButton.IsEnabled =
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
            StartClockButton.IsEnabled =
                ChangeTimeButton.IsEnabled = false;
            OnClockStarted(e);
        }

        private void StopClockButton_Click(object sender, RoutedEventArgs e)
        {
            StopClockButton.IsEnabled = false;
            StartClockButton.IsEnabled =
                ChangeTimeButton.IsEnabled = true;
            OnClockStopped(new ClockStoppedEventArgs { OffsetSet = false });
        }

        private void ChangeTimeButton_Click(object sender, RoutedEventArgs e)
        {
            var inputTime = new InputTimeWindow(Model.Matchclock);
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
            // TODO: real player list
            var goalInput = new GoalInputWindow(new[] { new Player { Name = "Fidde Sundström", PlayerNumber = 2 }, new Player { Name = "Anders Mogren", PlayerNumber = 8 } },
                Model.Matchclock);
            goalInput.Owner = Window.GetWindow(this);
            if (goalInput.ShowDialog() ?? false)
            {
                Debug.WriteLine($"Scorer: {goalInput.Player.Name}, time: {goalInput.MatchTime}");
                Model.HomeScore++;
                OnScoreChange(goalInput.MatchTime, goalInput.Player, true);
            }
        }

        private void HomeUndoGoalButton_Click(object sender, RoutedEventArgs e)
        {
            Model.HomeScore--;
            OnScoreChange(0, null, true, true);
        }

        private void AwayGoalScorerButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: real player list
            var goalInput = new GoalInputWindow(new[] { new Player { Name = "Fidde Sundström", PlayerNumber = 2 }, new Player { Name = "Anders Mogren", PlayerNumber = 8 } },
                Model.Matchclock);
            goalInput.Owner = Window.GetWindow(this);
            if (goalInput.ShowDialog() ?? false)
            {
                Debug.WriteLine($"Scorer: {goalInput.Player.Name}, time: {goalInput.MatchTime}");
                Model.AwayScore++;
                OnScoreChange(goalInput.MatchTime, goalInput.Player, false);
            }
        }

        private void AwayUndoGoalButton_Click(object sender, RoutedEventArgs e)
        {
            Model.AwayScore--;
            OnScoreChange(0, null, false, true);
        }

        #region Event handlers
        protected virtual void OnScoreChange(int matchTime, Player player, bool isHome, bool isCorrection = false)
        {
            var goal = new GoalEventArgs
            {
                Score = isHome ? Model.HomeScore : Model.AwayScore,
                MatchTime = matchTime,
                Player = player,
                IsHomeGoal = isHome,
                IsCorrection = isCorrection
            };
            ScoreChanged?.Invoke(this, goal);
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
