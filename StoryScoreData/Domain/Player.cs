using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoryScore.Data.Domain
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int PlayerNumber { get; set; }
        public string Position { get; set; }
        public string PicturePath { get; set; }
        public string PresentationVideoPath { get; set; }
        public string GoalVideoPath { get; set; }
        //[Column("Team_Id")]
        //public int TeamId { get; set; }

        public virtual Team Team { get; set; }
    }
}
