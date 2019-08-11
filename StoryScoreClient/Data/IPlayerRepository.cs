using StoryScore.Client.Model;
using System.Collections.Generic;

namespace StoryScore.Client.Data
{
    internal interface IPlayerRepository
    {
        Player GetPlayer(int id);
        IEnumerable<Player> GetPlayers(Team team);
        void RemovePlayer(Player player);
        void SavePlayer(Player player);
    }
}