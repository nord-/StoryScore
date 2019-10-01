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
        public ObservableCollection<PlayerViewModel> HomeLineUp { get; set; } = new ObservableCollection<PlayerViewModel>();
        public ObservableCollection<PlayerViewModel> AwayLineUp { get; set; } = new ObservableCollection<PlayerViewModel>();

        public TeamViewModel HomeTeam { get; set; }
        public TeamViewModel AwayTeam { get; set; }

        public PlayerViewModel SelectedHomePlayerPool { get; set; }
        public PlayerViewModel SelectedAwayPlayerPool { get; set; }
        public PlayerViewModel SelectedHomePlayerLineup { get; set; }
        public PlayerViewModel SelectedAwayPlayerLineup { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
