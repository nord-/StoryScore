using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScoreDisplay
{
    public class ScoreBoardModel
    {
        public string           Title        { get; set; }
        public int              HomeScore    { get; set; }
        public string           HomeTeamName { get; set; }
        public int              AwayScore    { get; set; }
        public string           AwayTeamName { get; set; }
        public int              ExtraTime    { get; set; }
        public string           PeriodName   { get; set; }
        public IList<GameEvent> HomeEvents   { get; set; }
        public IList<GameEvent> AwayEvents   { get; set; }

        public TimeSpan         GameClock    { get; set; }
        public string           GameClockDisplay
        {
            get
            {
                return $"{GameClock.TotalMinutes:00}:{GameClock.Seconds:00}";
            }
        }
    }
}
