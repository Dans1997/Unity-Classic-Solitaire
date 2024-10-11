using Enums;

namespace GameAnalytics.CustomEvents
{
    public class GameEndEvent : Unity.Services.Analytics.Event
    {
        public GameEndEvent() : base(CustomAnalyticsEvent.GameEnd.ToString()) { }
        
        public string GameOutcome { set => SetParameter("game_outcome", value); }
        public float TimeTaken { set => SetParameter("time_taken", value); }
        public int Score { set => SetParameter("score", value); }
        public int TotalMoves { set => SetParameter("total_moves", value); }
        public string DatePlayed { set => SetParameter("date_played", value); }
    }
}