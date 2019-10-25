using PropertyChanged;
using System;
using System.Collections.Generic;

namespace StoryScore.Display
{
    [AddINotifyPropertyChangedInterface]
    public class ScoreBoardModel
    {
        public string Title        { get; set; }
        public int    HomeScore    { get; set; }
        public string HomeTeamName { get; set; }
        public int    AwayScore    { get; set; }
        public string AwayTeamName { get; set; }
        public int    ExtraTime    { get; set; }
        public string PeriodName   { get; set; }

        public string ExtraTimeDisplay => ExtraTime == 0 ? "" : $"+{ExtraTime}";

        public IList<GameEvent> HomeEvents { get; set; }
        public IList<GameEvent> AwayEvents { get; set; }

        //public IList<Player MyProperty { get; set; }

        public TimeSpan GameClock { get; set; }
        public string GameClockDisplay => $"{Math.Floor(GameClock.TotalMinutes):00}:{GameClock.Seconds:00}";
    }
}
