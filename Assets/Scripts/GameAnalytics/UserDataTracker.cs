using System.Linq;
using GameAnalytics.CustomEvents;
using Interfaces;
using Unity.Services.Analytics;
using UnityEngine;

namespace GameAnalytics
{
    public class UserDataTracker : IUserDataTracker
    {
        public UserDataTracker(bool userConsented)
        {
            if (!userConsented)
            {
                Debug.LogWarning("User did not consent to data tracking");
                return;
            }
            
            AnalyticsService.Instance.StartDataCollection();
        }
        
        public void TrackGameStart(float gameStartTime)
        {
            var gameStartEvent = new GameStartEvent
            {
                StartTime = gameStartTime
            };
            AnalyticsService.Instance.RecordEvent(gameStartEvent);
        }

        public void TrackGameEnd(IGameResults gameResults)
        {
            var gameEndEvent = new GameEndEvent
            {
                GameOutcome = gameResults.GameOutcome.ToString(),
                TimeTaken = gameResults.TimeTaken,
                Score = gameResults.Score,
                TotalMoves = gameResults.TotalMoves,
                DatePlayed = gameResults.DatePlayed.ToString("o")
            };
            AnalyticsService.Instance.RecordEvent(gameEndEvent);
        }

        public void TrackCardMove(IGameMode gameMode, IGameMove gameMove, int moveNumber)
        {
            var cards = gameMove.Cards?.Select(card => card.CardType.ToString());
            var cardTypes = cards is not null ? string.Join(",", cards) : "";
            
            var cardMoveEvent = new CardMoveEvent
            {
                GameMode = gameMode.GameMode.ToString(),
                Cards = cardTypes,
                From = gameMove.Origin.PileType.ToString(),
                To = gameMove.Destination.PileType.ToString(),
                MoveNumber = moveNumber,
                TimeSinceGameStart = Time.time - gameMode.GameStartTime
            };
            AnalyticsService.Instance.RecordEvent(cardMoveEvent);
        }

        public void TrackUndoMove(IGameMode gameMode, IGameMove gameMove, int moveNumber)
        {
            var cards = gameMove.Cards?.Select(card => card.CardType.ToString());
            var cardTypes = cards is not null ? string.Join(",", cards) : "";
            
            var undoMoveEvent = new UndoMoveEvent
            {
                GameMode = gameMode.GameMode.ToString(),
                MoveName = gameMove.GetType().ToString(),
                Cards = cardTypes,
                From = gameMove.Origin.PileType.ToString(),
                To = gameMove.Destination.PileType.ToString(),
                MoveNumber = moveNumber,
                TimeSinceGameStart = Time.time - gameMode.GameStartTime
            };
            AnalyticsService.Instance.RecordEvent(undoMoveEvent);
        }

        public void TrackHintUsed(int hintCount, float gameStartTime)
        {
            var hintUsedEvent = new HintUseEvent
            {
                HintCount = hintCount,
                TimeSinceGameStart = Time.time - gameStartTime
            };
            AnalyticsService.Instance.RecordEvent(hintUsedEvent);
        }
    }
}
