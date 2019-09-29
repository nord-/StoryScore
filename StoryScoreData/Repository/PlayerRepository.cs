using System.Collections.Generic;
using System.Data.Entity;
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
            Context.Players.Remove(player);
        }

        public Player SavePlayer(Player player)
        {
            Context.Players.Attach(player);
            Context.SaveChanges();

            return player;
        }
    }
}
