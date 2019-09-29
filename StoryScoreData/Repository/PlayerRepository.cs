using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using StoryScore.Data.Domain;

namespace StoryScore.Data.Repository
{
    public class PlayerRepository : RepositoryBase, IPlayerRepository
    {
        public Player GetPlayer(int id)
        {
            return Context.Players.Find(id);
        }

        public IEnumerable<Player> GetPlayers(Team team)
        {
            return Context.Teams.Find(team.Id)
                               ?.Players;
        }

        public void RemovePlayer(Player player)
        {
            var p = Context.Players.Find(player.Id);
            Context.Players.Remove(p);
            Context.SaveChanges();
        }

        public Player SavePlayer(Player player)
        {
            Context.Set<Player>().AddOrUpdate(player);
            Context.SaveChanges();

            return player;
        }
    }
}
