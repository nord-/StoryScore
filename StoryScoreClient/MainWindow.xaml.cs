using Newtonsoft.Json;
using StoryScoreClient.Data;
using StoryScoreClient.Model;
using StoryScoreClient.Services;
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

namespace StoryScoreClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isAdding = false;
        private ITeamRepository _teamRepository;
        private readonly DisplayService _displayService;
        private readonly Options _options = new Options();
        private readonly Scoreboard _scoreboard = new Scoreboard();

        public MainWindow()
        {
            InitializeComponent();

            // TODO: Dep inject
            _teamRepository = new TeamRepository();
            _displayService = new DisplayService(new MqttClient(_options), _options);

            var teams = _teamRepository.GetTeams();
            TeamsList.ItemsSource = teams;

            TeamDetails.SaveClicked += TeamDetails_SaveClicked;
            TeamDetails.CancelClicked += TeamDetails_CancelClicked;

            MatchControls.Init(teams);
            MatchControls.ScoreChanged += Match_ScoreChanged;
            MatchControls.MatchStarted += Match_MatchStarted;
            MatchControls.ClockStarted += Match_ClockStarted;
            MatchControls.ClockStopped += Match_ClockStopped;
        }

        private async void Match_ClockStopped(object arg1, Controls.MatchControl.ClockStoppedEventArgs arg2)
        {
            if (arg2.OffsetSet)
                await _displayService.StopTimerAsync(arg2.TimerOffset);
            else
                await _displayService.StopTimerAsync();
        }

        private async void Match_ClockStarted(object arg1, EventArgs arg2)
        {
            await _displayService.StartTimerAsync();
        }

        private async void Match_MatchStarted(object arg1, EventArgs arg2)
        {
            // TODO: subscribe to clock event from display

            var model = MatchControls.Model;
            _scoreboard.HomeTeamName = model.HomeTeam.Name;
            _scoreboard.AwayTeamName = model.AwayTeam.Name;
            _scoreboard.HomeScore = model.HomeScore;
            _scoreboard.AwayScore = model.AwayScore;

            // send to display
            await _displayService.UpdateAsync(_scoreboard);
        }

        private async void Match_ScoreChanged(object arg1, EventArgs arg2)
        {
            _scoreboard.HomeScore = MatchControls.Model.HomeScore;
            _scoreboard.AwayScore = MatchControls.Model.AwayScore;

            await _displayService.UpdateAsync(_scoreboard);
        }

        private void TeamDetails_CancelClicked(object arg1, EventArgs arg2)
        {
            var theTeam = (Team)TeamDetails.DataContext;
            var teams = new List<Team>();
            teams.AddRange((IEnumerable<Team>)TeamsList.ItemsSource);

            if (isAdding)
            {
                var removeMe = teams.FirstOrDefault(t => t.Name == theTeam.Name);
                teams.Remove(removeMe);

                TeamsList.ItemsSource = teams;
            }

            TeamDetails.Visibility = Visibility.Hidden;
            TeamDetails.DataContext = null;

            isAdding = false;
            AddTeamButton.IsEnabled =
                RemoveTeamButton.IsEnabled =
                RenameTeamButton.IsEnabled =
                TeamsList.IsEnabled = true;
        }

        private void TeamDetails_SaveClicked(object arg1, EventArgs arg2)
        {
            var theTeam = (Team)TeamDetails.DataContext;
            TeamDetails.Visibility = Visibility.Hidden;
            TeamDetails.DataContext = null;

            AddTeamButton.IsEnabled =
                RemoveTeamButton.IsEnabled =
                RenameTeamButton.IsEnabled =
                TeamsList.IsEnabled = true;
            isAdding = false;

            // save the team to db
            _teamRepository.SaveTeam(theTeam);
        }

        private void TeamsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RenameTeamButton.IsEnabled = (TeamsList.SelectedIndex >= 0);
            if (!RenameTeamButton.IsEnabled)
            {
                TeamDetails.Visibility = Visibility.Hidden;
                TeamDetails.DataContext = null;
            }
            else
            {
                TeamDetails.DataContext = TeamsList.SelectedItem;
                TeamDetails.Visibility = Visibility.Visible;
            }
        }

        private void AddTeamButton_Click(object sender, RoutedEventArgs e)
        {
            var teams = new List<Team>();
            teams.AddRange((IEnumerable<Team>)TeamsList.ItemsSource);

            var newTeam = new Team { Name = "New team" };
            teams.Add(newTeam);

            TeamsList.ItemsSource = teams;
            TeamsList.SelectedItem = newTeam;

            isAdding = true;
            AddTeamButton.IsEnabled =
                RemoveTeamButton.IsEnabled =
                RenameTeamButton.IsEnabled =
                TeamsList.IsEnabled = false;

            TeamDetails.DataContext = newTeam;
            TeamDetails.Visibility = Visibility.Visible;
        }

        private void RenameTeamButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void RemoveTeamButton_Click(object sender, RoutedEventArgs e)
        {
            var theTeam = (Team)TeamsList.SelectedItem;
            _teamRepository.RemoveTeam(theTeam);
            var teams = (IList<Team>)TeamsList.ItemsSource;
            teams.Remove(theTeam);
            TeamsList.SelectedItem = null;
            TeamsList.ItemsSource = teams;
            TeamsList.Items.Refresh();
        }
    }
}
