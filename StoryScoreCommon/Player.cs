using Newtonsoft.Json;

namespace StoryScore.Common
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
        public int TeamId { get; set; }

        [JsonIgnore]
        public Team Team { get; set; }

        public string NameAndNumber => $"{PlayerNumber}. {Name}";
        public string NameNumberAndPosition
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Position))
                    return $"{NameAndNumber} ({Position})";
                else
                    return NameAndNumber;
            }
        }

    }
}
