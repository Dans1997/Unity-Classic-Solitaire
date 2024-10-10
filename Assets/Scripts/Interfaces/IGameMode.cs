using System.Collections;
using Utils;

namespace Interfaces
{
    public interface IGameMode
    {
        GameMode GameMode { get; }
        IEnumerator InitializeGame();
        IEnumerator RunGame();
    }
}