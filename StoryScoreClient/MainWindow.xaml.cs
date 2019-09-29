using AutoMapper;
using StoryScore.Client.Controls;
using StoryScore.Client.Model;
using StoryScore.Client.Services;
using StoryScore.Common;
using StoryScore.Data.Repository;
using StoryScore.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace StoryScore.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isAdding = false;
        private ITeamRepository _teamRepository;
        private ObservableCollection<TeamViewModel> _teams;
        private readonly DisplayService _displayService;
        private readonly Options _options = new Options();
        private readonly Scoreboard _scoreboard = new Scoreboard();

        public MainWindow()
        {
            InitializeComponent();

            // TODO: Dep inject
            _teamRepository = new TeamRepository();
            _displayService = new DisplayService(new MqttClient(_options), _options);
            _displayService.MatchClockTick += MatchClockTick;

            var teams = Mapper.Map<IEnumerable<TeamViewModel>>(_teamRepository.GetTeams());
            _teams = new ObservableCollection<TeamViewModel>(teams);
            TeamsList.ItemsSource = _teams;

            TeamDetails.SaveClicked        += TeamDetails_SaveClicked;
            TeamDetails.CancelClicked      += TeamDetails_CancelClicked;
            TeamDetails.ViewPlayersClicked += TeamDetails_ViewPlayersClicked;

            TeamPlayers.Close += TeamPlayers_Close;

            MatchControls.Init(_teams);
            MatchControls.ScoreChanged += Match_ScoreChanged;
            MatchControls.MatchStarted += Match_MatchStarted;
            MatchControls.ClockStarted += Match_ClockStarted;
            MatchControls.ClockStopped += Match_ClockStopped;
        }

        private void TeamPlayers_Close(PlayersControl obj)
        {
            TeamPlayers.Visibility = Visibility.Hidden;
            TeamDetails.Visibility = Visibility.Visible;

            var playerRepo = new PlayerRepository();
            var index = _teams.IndexOf((TeamViewModel)TeamsList.SelectedItem);
            // TODO: add players to view model
            //_teams.ElementAt(index).Players = playerRepo.GetPlayers(_teams.ElementAt(index))
            //                                            .ToList();
            TeamsList.ItemsSource = _teams;
            MatchControls.Init(_teams);
        }

        private void TeamDetails_ViewPlayersClicked(EditTeamControl target)
        {
            TeamDetails.Visibility = Visibility.Hidden;
            TeamPlayers.Visibility = Visibility.Visible;
            // load players
            TeamPlayers.Team = (TeamViewModel)TeamsList.SelectedItem;
            // TODO: add players to view model
            //TeamPlayers.Players = Mapper.Map<List<PlayerViewModel>>((TeamPlayers.Team).Players);
        }

        private void MatchClockTick(Heartbeat hb)
        {
            MatchControls.Model.Matchclock = hb.Matchclock;
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

        private async void Match_ScoreChanged(object sender, Controls.MatchControl.GoalEventArgs args)
        {
            if (args.IsHomeGoal)
                _scoreboard.HomeScore = args.Score;
            else
                _scoreboard.AwayScore = args.Score;

            await _displayService.UpdateGoalAsync(new Goal
            {
                Score = args.Score,
                IsHomeTeam = args.IsHomeGoal,
                IsCorrection = args.IsCorrection,
                ScorerName = args.Player?.Name,
                ScorerNumber = args.Player?.PlayerNumber ?? 0,
                ScorerImagePath = args.Player?.PicturePath,
                ScorerVideoPath = args.Player?.GoalVideoPath
            });
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
            var theTeam = (TeamViewModel)TeamDetails.DataContext;
            TeamDetails.Visibility = Visibility.Hidden;
            TeamDetails.DataContext = null;

            AddTeamButton.IsEnabled =
                RemoveTeamButton.IsEnabled =
                RenameTeamButton.IsEnabled =
                TeamsList.IsEnabled = true;
            isAdding = false;

            // save the team to db
            var team = _teamRepository.SaveTeam(Mapper.Map<Team>(theTeam));
            theTeam.Id = team.Id;
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
            var teams = new List<TeamViewModel>();
            teams.AddRange((IEnumerable<TeamViewModel>)TeamsList.ItemsSource);

            var newTeam = new TeamViewModel { Name = "New team" };
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
            var theTeam = (TeamViewModel)TeamsList.SelectedItem;
            _teamRepository.RemoveTeam(theTeam.Id);
            _teams.Remove(theTeam);
        }
    }
}
