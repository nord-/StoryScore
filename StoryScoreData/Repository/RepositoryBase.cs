namespace StoryScore.Data.Repository
{
    public class RepositoryBase
    {
        public StoryScoreContext Context { get; }

        public RepositoryBase()
        {
            Context = new StoryScoreContext();
        }
    }
}
