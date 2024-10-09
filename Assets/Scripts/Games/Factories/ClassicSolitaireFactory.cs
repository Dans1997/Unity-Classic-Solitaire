using Games.Modes;
using Interfaces;

namespace Games.Factories
{
    public class ClassicSolitaireFactory : IGameModeFactory
    {
        public IGameMode CreateGameMode()
        {
            return new ClassicSolitaireGameMode();
        }
    }
}