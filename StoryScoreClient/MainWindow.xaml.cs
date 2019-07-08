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
                var tempTeams = new [] { "FC Trollhättan", "AIK Skövde", "Presba Birlik", "IK Oddevold" };

                foreach (var item in tempTeams)
                {
                    TeamsList.Items.Add(item);
                }
            }
        }
    }
}
