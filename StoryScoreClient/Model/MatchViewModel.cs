using StoryScore.Data.Domain;
using System;
using System.ComponentModel;
using PropertyChanged;

namespace StoryScore.Client.Model
{
    [AddINotifyPropertyChangedInterface]
    public class MatchViewModel //: INotifyPropertyChanged
    {
        public TeamViewModel HomeTeam { get; set; }
        public TeamViewModel AwayTeam { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public TimeSpan Matchclock { get; set; }

        public string MatchclockDisplay
        {
            get
            {
                return $"{Math.Floor(Matchclock.TotalMinutes):00}:{Matchclock.Seconds:00}";
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;
    }
}
