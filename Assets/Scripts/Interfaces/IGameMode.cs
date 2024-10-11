using System;
using System.Collections;
using Utils;

namespace Interfaces
{
    public interface IGameMode
    {
        event Action PauseButtonClicked;
        event Action<IGameMode, IGameMove, int> MoveRegistered;
        event Action<IGameMode, IGameMove, int> MoveUndone;
        event Action<IGameResults> GameFinished;
        
        GameMode GameMode { get; }
        double GameStartTime { get; }

        IEnumerator DealCards();
        float StartGame();
    }
}