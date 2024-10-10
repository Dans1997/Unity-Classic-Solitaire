using Cards;

namespace Interfaces
{
    public interface IGameMove
    {
        Card Card { get; }
        CardColumn Origin { get; }
        CardColumn Destination { get; }
        bool WasFaceDown { get; }
        int Score { get; }

        void Execute();
        void Undo();
    }
}