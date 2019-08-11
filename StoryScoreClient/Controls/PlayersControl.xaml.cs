﻿using System;
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

namespace StoryScore.Client.Controls
{
    /// <summary>
    /// Interaction logic for PlayersControl.xaml
    /// </summary>
    public partial class PlayersControl : UserControl
    {
        public PlayersControl()
        {
            InitializeComponent();
        }

        private void AddPlayerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemovePlayerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlayersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditPlayerControl.DataContext = PlayersListBox.SelectedItem;
            EditPlayerControl.Visibility  = Visibility.Visible;
        }
    }
}
