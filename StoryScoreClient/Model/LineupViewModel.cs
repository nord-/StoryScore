using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        private ICommand _okClickCommand;
        public ICommand OkClickCommand => _okClickCommand ?? (_okClickCommand = new CommandHandler(() => OkClickAction(), () => OkCanExecute));
        // check if executing is allowed, i.e., validate, check if a process is running, etc.
        public bool OkCanExecute => true;

        private void OkClickAction()
        {
            System.Windows.MessageBox.Show("Hej!");
        }

        private ICommand _cancelClickCommand;
        public ICommand CancelClickCommand => _cancelClickCommand ?? (_cancelClickCommand = new CommandHandler(() => CancelClickAction(), () => true));

        private void CancelClickAction()
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
