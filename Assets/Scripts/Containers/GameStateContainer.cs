using Interfaces;

namespace Containers
{
    public class GameStateContainer
    {
        private readonly IGameMode gameMode;

        public GameStateContainer(IGameMode gameMode)
        {
            this.gameMode = gameMode;
        }

        public void StartGame()
        {
            gameMode?.InitializeGame();
        }

        public void UndoMove()
        {
            gameMode?.UndoLastMove();
        }
    }
}