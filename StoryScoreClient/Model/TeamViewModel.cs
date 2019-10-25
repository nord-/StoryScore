using AutoMapper;
using PropertyChanged;
using StoryScore.Data.Domain;
using System.Collections.ObjectModel;

namespace StoryScore.Client.Model
{
    [AddINotifyPropertyChangedInterface]
    public class TeamViewModel : Common.Team
    {
        public new ObservableCollection<PlayerViewModel> Players { get; set; }
    }

    public class TeamViewModelMappings : Profile
    {
        public TeamViewModelMappings()
        {
            CreateMap<Team, TeamViewModel>();

            CreateMap<TeamViewModel, Team>()
                .ForMember(dest => dest.Players, opt => opt.Ignore())
                ;
        }
    }
}
