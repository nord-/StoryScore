using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Common
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Coach { get; set; }
        public string ShortName { get; set; }
        public string LogoPath { get; set; }

        public IList<Player> Players { get; set; }

    }
}
