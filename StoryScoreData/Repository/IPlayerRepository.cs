using StoryScore.Data.Domain;
using System.Collections.Generic;

namespace StoryScore.Data.Repository
{
    internal interface IPlayerRepository
    {
        Player GetPlayer(int id);
        IEnumerable<Player> GetPlayers(Team team);
        void RemovePlayer(Player player);
        Player SavePlayer(Player player);
    }
}