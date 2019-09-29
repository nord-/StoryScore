using AutoMapper;
using StoryScore.Data.Domain;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StoryScore.Client.Model
{
    public class TeamViewModel : INotifyPropertyChanged
    {
        public int    Id { get; set; }
        public string Name { get; set; }
        public string Coach { get; set; }
        public string ShortName { get; set; }
        public string LogoPath { get; set; }

        public ObservableCollection<PlayerViewModel> Players { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
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
