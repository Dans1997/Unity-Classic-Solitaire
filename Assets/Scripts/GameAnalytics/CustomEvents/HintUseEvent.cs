using Enums;

namespace GameAnalytics.CustomEvents
{
    public class HintUseEvent : Unity.Services.Analytics.Event
    {
        public HintUseEvent() : base(CustomAnalyticsEvent.HintUse.ToString()) { }
        
        public int HintCount { set => SetParameter("hint_count", value); }
        public float TimeSinceGameStart { set => SetParameter("time_since_game_start", value); }
    }
}