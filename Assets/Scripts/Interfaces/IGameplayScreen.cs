using System;
using UnityEngine.UIElements;

namespace Interfaces
{
    public interface IGameplayScreen : IScreen
    {
        event Action SettingsButtonClicked;
        event Action ExitButtonClicked;
        event Action PauseButtonClicked;
        event Action UndoButtonClicked;
        
        VisualElement FindColumn(string className);
        void SetMoveCount(int movesCount);
        void SetTime(float time);
        void SetScore(int score);
    }
}