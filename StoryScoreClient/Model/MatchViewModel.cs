using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Client.Model
{
    public class MatchViewModel : ModelBase
    {
        private int _homeScore;
        private int _awayScore;
        private Team _homeTeam;
        private Team _awayTeam;

        public Team HomeTeam
        {
            get => _homeTeam;
            set
            {
                _homeTeam = value;
                NotifyChange(() => HomeTeam);
            }
        }

        public Team AwayTeam
        {
            get => _awayTeam;
            set
            {
                _awayTeam = value;
                NotifyChange(() => AwayTeam);
            }
        }

        public int HomeScore
        {
            get => _homeScore;
            set
            {
                _homeScore = value;
                NotifyChange(() => HomeScore);
            }
        }

        public int AwayScore
        {
            get => _awayScore;
            set
            {
                _awayScore = value;
                NotifyChange(() => AwayScore);
            }
        }
    }
}
