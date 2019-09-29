using StoryScore.Data.Domain;
using System.Collections.Generic;

namespace StoryScore.Data.Repository
{
    public interface IPlayerRepository
    {
        Player GetPlayer(int id);
        IEnumerable<Player> GetPlayers(Team team);
        void RemovePlayer(Player player);
        Player SavePlayer(Player player);
    }
}