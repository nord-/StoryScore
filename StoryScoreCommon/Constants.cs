using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Common.Constants
{
    public class Mqtt
    {
        public const string Start = "start";
        public const string Update = "update";
        public const string Stop = "stop";
        public const string Goal = "goal";
        public const string Status = "status";
    }

    public class Topic
    {
        public const string Display = "display";
        public const string Sync = "sync";
    }
}
