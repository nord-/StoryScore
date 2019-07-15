﻿using StoryScoreClient.Model;
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
        public MainWindow()
        {
            InitializeComponent();

            var teams = new Data.TeamRepository().GetTeams();
            if (teams.Count() == 0)
            {
                teams = new[]
                {
                    new Team { Id = 1, Name = "FC Trollhättan", ShortName = "FCT", Coach = "William Lundin", LogoPath = @"D:\Users\Rickard\OneDrive\Bilder\FCT\fct_logo.png" },
                    new Team { Id = 2, Name = "AIK Skövde", ShortName = "SAIK", Coach = "Karl Kragballe", LogoPath = @"D:\Users\Rickard\OneDrive\Bilder\FCT\fct.png" },
                };
            }

            TeamsList.ItemsSource = teams;

            TeamDetails.SaveClicked += TeamDetails_SaveClicked;
            TeamDetails.CancelClicked += TeamDetails_CancelClicked;
        }

        private void TeamDetails_CancelClicked(object arg1, EventArgs arg2)
        {
            var theTeam = (Team)TeamDetails.DataContext;
            var teams = new List<Team>();
            teams.AddRange((IEnumerable<Team>)TeamsList.ItemsSource);

            var removeMe = teams.FirstOrDefault(t => t.Name == theTeam.Name);
            teams.Remove(removeMe);

            TeamsList.ItemsSource = teams;

            TeamDetails.Visibility = Visibility.Hidden;
            TeamDetails.DataContext = null;

            AddTeamButton.IsEnabled = true;
        }

        private void TeamDetails_SaveClicked(object arg1, EventArgs arg2)
        {
            var theTeam = (Team)TeamDetails.DataContext;
            TeamDetails.Visibility = Visibility.Hidden;
            TeamDetails.DataContext = null;

            AddTeamButton.IsEnabled = true;

            // TODO: save the team to db
        }

        private void TeamsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RenameTeamButton.IsEnabled = (TeamsList.SelectedIndex >= 0);
            TeamDetails.Visibility = Visibility.Hidden;
            TeamDetails.DataContext = null;
        }

        private void AddTeamButton_Click(object sender, RoutedEventArgs e)
        {
            var teams = new List<Team>();
            teams.AddRange((IEnumerable<Team>)TeamsList.ItemsSource);

            var newTeam = new Team { Name = "New team" };
            teams.Add(newTeam);

            TeamsList.ItemsSource = teams;
            ((Button)sender).IsEnabled = false;

            TeamDetails.DataContext = newTeam;
            TeamDetails.Visibility = Visibility.Visible;
        }

        private void RenameTeamButton_Click(object sender, RoutedEventArgs e)
        {
            TeamDetails.DataContext = TeamsList.SelectedItem;
            TeamDetails.Visibility = Visibility.Visible;
        }


    }
}
