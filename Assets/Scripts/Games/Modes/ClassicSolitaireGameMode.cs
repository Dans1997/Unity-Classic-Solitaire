using System.Collections.Generic;
using Interfaces;

namespace Games.Modes
{
    public class ClassicSolitaireGameMode : IGameMode
    {
        private readonly Stack<string> moves;

        public ClassicSolitaireGameMode()
        {
            moves = new Stack<string>();
        }

        public void InitializeGame()
        {
            // Initialization of Classic Solitaire
        }

        public void UndoLastMove()
        {
            if (moves.Count > 0)
            {
                moves.Pop();
                // Handle Undo logic here
            }
        }

        public void RegisterMove(string move)
        {
            moves.Push(move);
        }
    }
}