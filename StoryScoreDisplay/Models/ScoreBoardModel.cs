using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StoryScoreDisplay
{
    public class ScoreBoardModel { }

    public class ScoreBoard : INotifyPropertyChanged
    {
        private string title;
        private int homeScore;
        private string homeTeamName;
        private int awayScore;
        private string awayTeamName;
        private int extraTime;
        private string periodName;

        public string Title
        {
            get => title;
            set
            {
                if (value != title)
                {
                    title = value;
                    NotifyPropertyChanged(nameof(Title));
                }
            }
        }
        public int HomeScore
        {
            get => homeScore;
            set
            {
                if (value != homeScore)
                {
                    homeScore = value;
                    NotifyPropertyChanged(nameof(HomeScore));
                }
            }
        }
        public string HomeTeamName
        {
            get => homeTeamName;
            set
            {
                if (value != homeTeamName)
                {
                    homeTeamName = value;
                    NotifyPropertyChanged(nameof(HomeTeamName));
                }
            }
        }
        public int AwayScore
        {
            get => awayScore;
            set
            {
                if (value != awayScore)
                {
                    awayScore = value;
                    NotifyPropertyChanged(nameof(AwayScore));
                }
            }
        }
        public string AwayTeamName
        {
            get => awayTeamName;
            set
            {
                if (value != awayTeamName)
                {
                    awayTeamName = value;
                    NotifyPropertyChanged(nameof(AwayTeamName));
                }
            }
        }
        public int ExtraTime
        {
            get => extraTime;
            set
            {
                if (value != extraTime)
                {
                    extraTime = value;
                    NotifyPropertyChanged(nameof(ExtraTime));
                }
            }

        }
        public string PeriodName
        {
            get => periodName;
            set
            {
                if (value != periodName)
                {
                    periodName = value;
                    NotifyPropertyChanged(nameof(PeriodName));
                }
            }
        }
        public IList<GameEvent> HomeEvents { get; set; }
        public IList<GameEvent> AwayEvents { get; set; }

        public TimeSpan GameClock { get; set; }
        public string GameClockDisplay
        {
            get
            {
                return $"{GameClock.TotalMinutes:00}:{GameClock.Seconds:00}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
