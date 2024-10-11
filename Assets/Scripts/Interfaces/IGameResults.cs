using System;
using Enums;

namespace Interfaces
{
    public interface IGameResults
    {
        GameOutcome GameOutcome { get; }
        float TimeTaken { get; }
        int Score { get; }
        int TotalMoves { get; }
        DateTime DatePlayed { get; }
    }
}