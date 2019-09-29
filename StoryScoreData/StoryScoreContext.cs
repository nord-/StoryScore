using StoryScore.Data.Domain;
using System.Data.Entity;

namespace StoryScore.Data
{
    public class StoryScoreContext : DbContext
    {
        public StoryScoreContext() : base("StoryScoreDB") {}

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}
