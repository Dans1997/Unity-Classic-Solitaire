using Enums;

namespace GameAnalytics.CustomEvents
{
    public class GameStartEvent : Unity.Services.Analytics.Event
    {
        public GameStartEvent() : base(CustomAnalyticsEvent.GameStart.ToString()) { }
        
        public float StartTime { set => SetParameter("start_time", value); }
    }
}