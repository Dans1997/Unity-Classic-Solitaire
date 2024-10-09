using System;

namespace Interfaces
{
    public interface IGameplayScreen : IScreen
    {
        event Action SettingsButtonClicked;
        event Action ExitButtonClicked;
        event Action PauseButtonClicked;
        event Action UndoButtonClicked;
    }
}