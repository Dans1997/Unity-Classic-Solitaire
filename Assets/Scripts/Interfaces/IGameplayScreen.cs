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
    }
}