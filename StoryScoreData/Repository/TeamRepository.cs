using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using StoryScore.Client.Data;
using StoryScore.Data.Domain;

namespace StoryScore.Data.Repository
{
    public class TeamRepository : RepositoryBase, ITeamRepository
    {
        public Team GetTeam(int id)
        {
            return Context.Teams.Find(id);
        }

        public IEnumerable<Team> GetTeams()
        {
            return Context.Teams
                          .Include(t => t.Players)
                          ;
        }

        public Team SaveTeam(Team team)
        {
            if (team.Id != 0)
                Context.Teams.Attach(team);
            else
                Context.Teams.Add(team);

            Context.SaveChanges();

            return team;
        }

        public void RemoveTeam(Team team)
        {
            //Context.Teams.Attach(team);

            if (team.Players.AnyEx())
            {
                foreach (var player in team.Players.ToList())
                    Context.Players.Remove(player);
            }

            Context.Teams.Remove(team);
            Context.SaveChanges();
        }

        public void RemoveTeam(int id)
        {
            var team = Context.Teams.Find(id);
            RemoveTeam(team);
        }
    }
}
