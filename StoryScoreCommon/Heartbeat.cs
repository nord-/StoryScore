using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Common
{
    public class Heartbeat
    {
        public string DisplayId { get; set; }
        public TimeSpan Matchclock { get; set; }
    }
}
