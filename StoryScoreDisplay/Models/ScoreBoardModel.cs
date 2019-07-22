using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Display
{
    public class ScoreBoardModel : INotifyPropertyChanged
    {
        private string _title;
        private int _homeScore;
        private string _homeTeamName;
        private int _awayScore;
        private string _awayTeamName;
        private int _extraTime;
        private string _periodName;
        private TimeSpan _gameClock;

        public string Title
        {
            get => _title;
            set
            {
                if (value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged(nameof(Title));
                }
            }
        }
        public int HomeScore
        {
            get => _homeScore;
            set
            {
                if (value != _homeScore)
                {
                    _homeScore = value;
                    NotifyPropertyChanged(nameof(HomeScore));
                }
            }
        }
        public string HomeTeamName
        {
            get => _homeTeamName;
            set
            {
                if (value != _homeTeamName)
                {
                    _homeTeamName = value;
                    NotifyPropertyChanged(nameof(HomeTeamName));
                }
            }
        }
        public int AwayScore
        {
            get => _awayScore;
            set
            {
                if (value != _awayScore)
                {
                    _awayScore = value;
                    NotifyPropertyChanged(nameof(AwayScore));
                }
            }
        }
        public string AwayTeamName
        {
            get => _awayTeamName;
            set
            {
                if (value != _awayTeamName)
                {
                    _awayTeamName = value;
                    NotifyPropertyChanged(nameof(AwayTeamName));
                }
            }
        }
        public int ExtraTime
        {
            get => _extraTime;
            set
            {
                if (value != _extraTime)
                {
                    _extraTime = value;
                    NotifyPropertyChanged(nameof(ExtraTime));
                    NotifyPropertyChanged(nameof(ExtraTimeDisplay));
                }
            }
        }
        public string ExtraTimeDisplay => _extraTime == 0 ? "" : $"+{_extraTime}";

        public string PeriodName
        {
            get => _periodName;
            set
            {
                if (value != _periodName)
                {
                    _periodName = value;
                    NotifyPropertyChanged(nameof(PeriodName));
                }
            }
        }
        public IList<GameEvent> HomeEvents { get; set; }
        public IList<GameEvent> AwayEvents { get; set; }

        public TimeSpan GameClock
        {
            get => _gameClock;
            set
            {
                _gameClock = value;
                NotifyPropertyChanged(nameof(GameClock));
                NotifyPropertyChanged(nameof(GameClockDisplay));
            }
        }
        public string GameClockDisplay
        {
            get
            {
                return $"{Math.Floor(GameClock.TotalMinutes):00}:{GameClock.Seconds:00}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
