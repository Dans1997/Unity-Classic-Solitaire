using Enums;

namespace GameAnalytics.CustomEvents
{
    public class CardMoveEvent : Unity.Services.Analytics.Event
    {
        public CardMoveEvent() : base(CustomAnalyticsEvent.CardMove.ToString()) { }
        
        public string GameMode { set => SetParameter("game_mode", value); }
        public string Cards { set => SetParameter("cards", value); }
        public string From { set => SetParameter("from", value); }
        public string To { set => SetParameter("to", value); }
        public int MoveNumber { set => SetParameter("move_number", value); }
        public double TimeSinceGameStart { set => SetParameter("time_since_game_start", value); }
    }
}