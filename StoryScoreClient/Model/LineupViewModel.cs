using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Client.Model
{
    public class LineupViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TeamViewModel> Teams { get; set; }
        public ObservableCollection<PlayerViewModel> HomePlayers { get; set; }
        public ObservableCollection<PlayerViewModel> HomeLineUp { get; set; }
        public ObservableCollection<PlayerViewModel> AwayPlayers { get; set; }
        public ObservableCollection<PlayerViewModel> AwayLineUp { get; set; }

        public TeamViewModel HomeTeam { get; set; }
        public TeamViewModel AwayTeam { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
