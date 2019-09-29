using System;
using System.Collections.Generic;

namespace StoryScore.Data.Domain
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Coach { get; set; }
        public string ShortName { get; set; }
        public string LogoPath { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}
