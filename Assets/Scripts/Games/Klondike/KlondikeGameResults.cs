using System;
using Enums;
using Interfaces;

namespace Games.Klondike
{
    public class KlondikeGameResults : IGameResults
    {
        public GameOutcome GameOutcome { get; }
        public float TimeTaken { get; }
        public int Score { get; }
        public int TotalMoves { get; }
        public DateTime DatePlayed { get; }
        
        public KlondikeGameResults(GameOutcome outcome, float timeTaken, int score, int totalMoves)
        {
            GameOutcome = outcome;
            TimeTaken = timeTaken;
            Score = score;
            TotalMoves = totalMoves;
            DatePlayed = DateTime.Now;
        }
        
        public override string ToString()
        {
            return $"Game Outcome: {GameOutcome}\n" +
                   $"Time Taken: {TimeTaken} seconds\n" +
                   $"Score: {Score}\n" +
                   $"Total Moves: {TotalMoves}\n" +
                   $"Date Played: {DatePlayed}\n";
        }
    }
}