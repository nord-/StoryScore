﻿using StoryScore.Client.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Client.Data
{
    public interface ITeamRepository
    {
        Team GetTeam(int id);
        IEnumerable<Team> GetTeams();
        void RemoveTeam(Team team);
        void SaveTeam(Team team);
    }
}
