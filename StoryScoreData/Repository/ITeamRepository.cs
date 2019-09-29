using StoryScore.Data.Domain;
using System.Collections.Generic;

namespace StoryScore.Data.Repository
{
    public interface ITeamRepository
    {
        Team GetTeam(int id);
        IEnumerable<Team> GetTeams();
        void RemoveTeam(Team team);
        void RemoveTeam(int id);
        Team SaveTeam(Team team);
    }
}
