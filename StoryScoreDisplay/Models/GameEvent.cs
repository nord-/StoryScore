namespace StoryScoreDisplay
{
    public class GameEvent
    {
        public int Index { get; set; }
        public string EventTime { get; set; }
        public string Text { get; set; }
        public EventTypeEnum EventType { get; set; }
    }

    public enum EventTypeEnum
    {
        Goal,
        YellowCard,
        RedCard,
        Substitution,
        Penalty
    }
}