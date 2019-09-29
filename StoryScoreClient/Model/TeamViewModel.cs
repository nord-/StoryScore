using AutoMapper;
using StoryScore.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Client.Model
{
    public class TeamViewModel
    {
        public ObservableProperty<int> Id { get; set; } = new ObservableProperty<int>();
        public ObservableProperty<string> Name { get; set; } = new ObservableProperty<string>();
        public ObservableProperty<string> Coach { get; set; } = new ObservableProperty<string>();
        public ObservableProperty<string> ShortName { get; set; } = new ObservableProperty<string>();
        public ObservableProperty<string> LogoPath { get; set; } = new ObservableProperty<string>();
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
