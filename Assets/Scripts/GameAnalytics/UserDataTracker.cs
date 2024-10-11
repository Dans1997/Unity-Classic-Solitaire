using System.Collections.Generic;
using System.Linq;
using Enums;
using Interfaces;
using UnityEngine;
using UnityEngine.Analytics;

namespace GameAnalytics
{
    public class UserDataTracker : IUserDataTracker
    {
        public UserDataTracker()
        {
            Analytics.enabled = true;
            Analytics.initializeOnStartup = true;
            Analytics.deviceStatsEnabled = true;
        }
        
        public void TrackGameStart(float gameStartTime)
        {
            Analytics.CustomEvent(CustomAnalyticsEvent.GameStart.ToString(), new Dictionary<string, object>
            {
                { "start_time", gameStartTime }
            });
        }

        public void TrackGameEnd(IGameResults gameResults)
        {
            Analytics.CustomEvent(CustomAnalyticsEvent.GameEnd.ToString(), new Dictionary<string, object>
            {
                { "game_outcome", gameResults.GameOutcome.ToString() },
                { "time_taken", gameResults.TimeTaken },
                { "score", gameResults.Score },
                { "total_moves", gameResults.TotalMoves },
                { "date_played", gameResults.DatePlayed.ToString("o") }
            });
        }
        
        public void TrackCardMove(IGameMode gameMode, IGameMove gameMove, int moveNumber)
        {
            var cards = gameMove.Cards?.Select(card => card.CardType.ToString());
            var cardTypes = cards is not null ? string.Join(",", cards) : "";
            
            Analytics.CustomEvent(CustomAnalyticsEvent.CardMove.ToString(), new Dictionary<string, object>
            {
                { "game_mode", gameMode.GameMode.ToString() },
                { "cards", cardTypes },
                { "from", gameMove.Origin.name },
                { "to", gameMove.Destination.name },
                { "move_number", moveNumber },
                { "time_since_game_start", Time.time - gameMode.GameStartTime }
            });
        }
        
        public void TrackUndoMove(IGameMode gameMode, IGameMove gameMove, int moveNumber)
        {
            var cards = gameMove.Cards?.Select(card => card.CardType.ToString());
            var cardTypes = cards is not null ? string.Join(",", cards) : "";
            
            Analytics.CustomEvent(CustomAnalyticsEvent.UndoMove.ToString(), new Dictionary<string, object>
            {
                { "game_mode", gameMode.GameMode.ToString() },
                { "move_name", gameMove.GetType().ToString() },
                { "cards", cardTypes },
                { "from", gameMove.Origin.name },
                { "to", gameMove.Destination.name },
                { "move_number", moveNumber },
                { "time_since_game_start", Time.time - gameMode.GameStartTime }
            });
        }
        
        // TODO:
        public void TrackHintUsed(int hintCount, float gameStartTime)
        {
            Analytics.CustomEvent(CustomAnalyticsEvent.HintUse.ToString(), new Dictionary<string, object>
            {
                { "hint_count", hintCount },
                { "time_since_game_start", Time.time - gameStartTime }
            });
        }
    }
}
