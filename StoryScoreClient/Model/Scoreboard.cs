using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScoreClient.Model
{
    public class Scoreboard
    {
        public string Title { get; set; }
        public int HomeScore { get; set; }
        public string HomeTeamName { get; set; }
        public int AwayScore { get; set; }
        public string AwayTeamName { get; set; }
        public int ExtraTime { get; set; }
        public string PeriodName { get; set; }
        public TimeSpan GameClock { get; set; }
    }
}
