using System;
using System.Collections.Generic;
using System.IO;
using Dapper.Contrib.Extensions;
using StoryScoreClient.Model;

namespace StoryScoreClient.Data
{
    public class TeamRepository : SqLiteBaseRepository, ITeamRepository
    {
        public Team GetTeam(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Team> GetTeams()
        {
            using (var cn = StoryScoreDbConnection())
            {
                return cn.GetAll<Team>();
            }
        }

        public void SaveTeam(Team team)
        {
            throw new NotImplementedException();
        }
    }
}
