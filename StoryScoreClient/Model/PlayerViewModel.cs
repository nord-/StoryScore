using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Client.Model
{
    public class PlayerViewModel : ModelBase
    {
        private int    _id;
        private string _name;
        private int    _playerNumber;
        private string _position;
        private string _picturePath;
        private string _presentationVideoPath;
        private string _goalVideoPath;

        public int    Id                    { get => _id; set { _id = value; NotifyChange(() => Id); } }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyChange(() => Name);
                NotifyChange(() => NameAndNumber);
            }
        }
        public int PlayerNumber
        {
            get => _playerNumber;
            set
            {
                _playerNumber = value;
                NotifyChange(() => PlayerNumber);
                NotifyChange(() => NameAndNumber);
            }
        }
        public string Position              { get => _position;              set { _position = value; NotifyChange(() => Position); } }
        public string PicturePath           { get => _picturePath;           set { _picturePath = value; NotifyChange(() => PicturePath); } }
        public string PresentationVideoPath { get => _presentationVideoPath; set { _presentationVideoPath = value; NotifyChange(() => PresentationVideoPath); } }
        public string GoalVideoPath         { get => _goalVideoPath;         set { _goalVideoPath = value; NotifyChange(() => GoalVideoPath); } }
        public Team Team { get; set; }

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
