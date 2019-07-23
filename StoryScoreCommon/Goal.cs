using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Common
{
    public class Goal
    {
        public int Score { get; set; }
        public bool IsHomeTeam { get; set; }
        public string ScorerName { get; set; }
        public int ScorerNumber { get; set; }
        public string ScorerImagePath { get; set; }
        public string ScorerVideoPath { get; set; }
        public bool IsCorrection { get; set; }
    }
}
