using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace StoryScoreClient.Model
{
    public class Team : ModelBase
    {
        private int _id;
        private string _name;
        private string _coach;
        private string _shortName;
        private string _logoPath;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                NotifyChange(() => Id);
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyChange(() => Name);
            }
        }
        public string Coach
        {
            get => _coach;
            set
            {
                _coach = value;
                NotifyChange(() => Coach);
            }
        }
        public string ShortName
        {
            get => _shortName;
            set
            {
                _shortName = value;
                NotifyChange(() => ShortName);
            }
        }
        public string LogoPath
        {
            get => _logoPath;
            set
            {
                _logoPath = value;
                NotifyChange(() => LogoPath);
            }
        }

        [Write(false)]
        public virtual ICollection<Player> Players { get; set; }
    }
}
