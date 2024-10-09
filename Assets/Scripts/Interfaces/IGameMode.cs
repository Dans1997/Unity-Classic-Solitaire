using System.Collections;

namespace Interfaces
{
    public interface IGameMode
    {
        IEnumerator InitializeGame();
        IEnumerator RunGame();
    }
}