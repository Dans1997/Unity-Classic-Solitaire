using System.Collections.Generic;
using Cards;

namespace Interfaces
{
    public interface IGameMove
    {
        List<Card> Cards { get; }
        CardColumn Origin { get; }
        CardColumn Destination { get; }
        bool WasFaceDown { get; }
        int Score { get; }

        void Execute();
        void Undo();
    }
}