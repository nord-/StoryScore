using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StoryScoreDisplay
{
    public class GameEvent : INotifyPropertyChanged
    {
        public int Index { get; set; }
        public string EventTime { get; set; }
        public string Text { get; set; }
        public EventTypeEnum EventType { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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