using Cards;
using Enums;

namespace Interfaces
{
    public interface IGameRules
    {
        int ColumnCount { get; }
        
        bool CanMoveToFoundation(Card cardToMove, CardColumn foundation);
        bool CanMoveToTableau(Card cardToMove, CardColumn tableau);
        bool HasValidMoves(CardColumn stockPile, CardColumn stockPileDump, CardColumn[] tableauPiles,
            CardColumn[] foundationPiles);
        bool IsGameEnd(CardColumn stockPile, CardColumn stockPileDump, CardColumn[] tableauPiles,
            CardColumn[] foundationPiles, out GameOutcome gameOutcome);
    }
}