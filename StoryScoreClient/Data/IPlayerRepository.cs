using StoryScore.Client.Model;
using System.Collections.Generic;

namespace StoryScore.Client.Data
{
    internal interface IPlayerRepository
    {
        Player GetPlayer(int id);
        IEnumerable<Player> GetPlayers(int teamId);
        void RemovePlayer(Player player);
        void SavePlayer(Player player);
    }
}