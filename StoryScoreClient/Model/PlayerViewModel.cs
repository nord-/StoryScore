using AutoMapper;
using StoryScore.Data.Domain;
using System.ComponentModel;

namespace StoryScore.Client.Model
{
    public class PlayerViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        // [AlsoNotifyFor("NameAndNumber")]
        public string Name { get; set; }
        // [AlsoNotifyFor("NameAndNumber")]
        public int PlayerNumber { get; set; }
        public string Position { get; set; }
        public string PicturePath { get; set; }
        public string PresentationVideoPath { get; set; }
        public string GoalVideoPath { get; set; }
        public int TeamId { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class PlayerViewModelMappings : Profile
    {
        public PlayerViewModelMappings()
        {
            CreateMap<Player, PlayerViewModel>()
                ;

            CreateMap<PlayerViewModel, Player>()
                //.ForMember(dest => dest.Team, opt => opt.Ignore())
                ;
        }
    }

}
