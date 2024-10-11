using System;
using System.Collections;
using Utils;

namespace Interfaces
{
    public interface IGameMode
    {
        public event Action<IGameResults> GameFinished;
        GameMode GameMode { get; }
        IEnumerator DealCards();
        void StartGame();
    }
}