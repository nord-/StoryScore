using AutoMapper;
using StoryScore.Data.Domain;

namespace StoryScore.Client.Model
{
    public class PlayerViewModel : ModelBase
    {
        //public string Name
        //{
        //    get => _name;
        //    set
        //    {
        //        _name = value;
        //        NotifyChange(()             => Name);
        //        NotifyChange(()             => NameAndNumber);
        //    }
        //}
        //public int PlayerNumber
        //{
        //    get => _playerNumber;
        //    set
        //    {
        //        _playerNumber               = value;
        //        NotifyChange(()             => PlayerNumber);
        //        NotifyChange(()             => NameAndNumber);
        //    }
        //}
        public ObservableProperty<int>        Id { get; set; }
        public ObservableProperty<string>     Name { get; set; }
        public ObservableProperty<int>        PlayerNumber { get; set; }
        public ObservableProperty<string>     Position { get; set; }
        public ObservableProperty<string>     PicturePath { get; set; }
        public ObservableProperty<string>     PresentationVideoPath { get; set; }
        public ObservableProperty<string>     GoalVideoPath { get; set; }
        public Team Team { get;               set; }

        public string NameAndNumber => $"{PlayerNumber}. {Name}";
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
