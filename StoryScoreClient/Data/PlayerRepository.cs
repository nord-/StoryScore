using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using StoryScore.Client.Model;

namespace StoryScore.Client.Data
{
    public class PlayerRepository : SqLiteBaseRepository, IPlayerRepository
    {
        public Player GetPlayer(int id)
        {
            using (var cn = StoryScoreDbConnection())
            {
                return cn.Get<Player>(id);
            }
        }

        public IEnumerable<Player> GetPlayers(Team team)
        {
            const string Sql = "SELECT * FROM Players WHERE TeamId=@teamId ORDER BY PlayerNumber";
            using (var cn = StoryScoreDbConnection())
            {
                var players = cn.Query<Player>(Sql, new { teamId = team.Id });
                foreach (var p in players)
                    p.Team = team;

                return players;
            }
        }

        public void RemovePlayer(Player player)
        {
            using (var cn = StoryScoreDbConnection())
            {
                cn.Delete(player);
            }
        }

        public void SavePlayer(Player player)
        {
            using (var cn = StoryScoreDbConnection())
            {
                if (player.Id != 0)
                {
                    // update
                    cn.Update<Player>(player);
                }
                else
                {
                    // insert
                    cn.Insert(player);
                }
            }
        }
    }
}
