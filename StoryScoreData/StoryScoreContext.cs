using StoryScore.Data.Domain;
using System.Data.Entity;

namespace StoryScore.Data
{
    public class StoryScoreContext : DbContext
    {
        public StoryScoreContext() : base("StoryScoreDB")
        {
            //Database.SetInitializer<StoryScoreContext>(null);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<StoryScoreContext, Migrations.Configuration>());
            Database.Initialize(false);
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
