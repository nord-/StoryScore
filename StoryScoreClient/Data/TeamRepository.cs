using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using StoryScore.Client.Model;

namespace StoryScore.Client.Data
{
    public class TeamRepository : SqLiteBaseRepository, ITeamRepository
    {
        public Team GetTeam(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Team> GetTeams()
        {
            const string sql = @"select t.*, p.id ""PlayerId"", p.name ""PlayerName"", p.* from teams t left join players p on t.id = p.teamid order by t.name, p.playernumber";
            using (var cn = StoryScoreDbConnection())
            {
                var teamsDictionary = new Dictionary<int, Team>();

                var teams = cn.Query<Team, Player, Team>(sql,
                    (team, player) => {
                        if (!teamsDictionary.TryGetValue(team.Id, out var teamEntry))
                        {
                            teamEntry = team;
                            teamEntry.Players = new List<Player>();
                            teamsDictionary.Add(teamEntry.Id, teamEntry);
                        }

                        if (player != null)
                            player.Team = teamEntry;

                        if (player != null && player.Id != 0)
                            teamEntry.Players.Add(player);
                        return teamEntry;
                    },
                    splitOn: "PlayerId")
                    .Distinct()
                    .ToList();

                return teams;
            }
        }

        public void SaveTeam(Team team)
        {
            using (var cn = StoryScoreDbConnection())
            {
                if (team.Id != 0)
                {
                    // update
                    cn.Update<Team>(team);
                }
                else
                {
                    // insert
                    cn.Insert(team);
                }
            }
        }

        public void RemoveTeam(Team team)
        {
            using (var cn = StoryScoreDbConnection())
            {
                // if players, remove them first --> no orphans!
                if (team.Players.AnyEx())
                {
                    foreach (var player in team.Players)
                    {
                        cn.Delete(player);
                    }
                }

                cn.Delete(team);
            }
        }
    }
}
