using PropertyChanged;
using StoryScore.Common;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace StoryScore.Display
{
    [AddINotifyPropertyChangedInterface]
    public class ScoreBoardModel
    {
        private string _adFilePath;
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

        public IList<Player> HomePlayers { get; set; }
        public IList<Player> AwayPlayers { get; set; }

        public TimeSpan GameClock { get; set; }
        public string GameClockDisplay => $"{Math.Floor(GameClock.TotalMinutes):00}:{GameClock.Seconds:00}";
    }
}
