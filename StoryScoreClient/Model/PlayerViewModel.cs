using AutoMapper;
using PropertyChanged;
using StoryScore.Data.Domain;

namespace StoryScore.Client.Model
{
    [AddINotifyPropertyChangedInterface]
    public class PlayerViewModel : Common.Player
    {
    }

    public class PlayerViewModelMappings : Profile
    {
        public PlayerViewModelMappings()
        {
            CreateMap<Team, Common.Team>();
            CreateMap<Common.Team, Team>();

            CreateMap<Player, Common.Player>();
            CreateMap<Common.Player, Player>();
            CreateMap<Player, PlayerViewModel>();
            CreateMap<PlayerViewModel, Player>();
        }
    }

}
