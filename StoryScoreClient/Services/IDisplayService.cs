using StoryScore.Client.Model;
using StoryScore.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoryScore.Client.Services
{
    public interface IDisplayService
    {
        event Action<Heartbeat> MatchClockTick;

        Task SendLineupAsync(IEnumerable<PlayerViewModel> homeLineUp, IEnumerable<PlayerViewModel> awayLineUp);
        Task StartTimerAsync();
        Task StopTimerAsync();
        Task StopTimerAsync(TimeSpan offset);
        Task UpdateAsync(Scoreboard scoreboard);
        Task UpdateGoalAsync(Goal goal);
    }
}