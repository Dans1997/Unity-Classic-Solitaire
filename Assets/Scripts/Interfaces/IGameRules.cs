using System.Collections.Generic;
using Cards;
using Enums;

namespace Interfaces
{
    public interface IGameRules
    {
        int ColumnCount { get; }
        
        bool CanMoveToFoundation(List<Card> cardsToMove, CardColumn foundation);
        bool CanMoveToTableau(List<Card> cardsToMove, CardColumn tableau);
        bool HasValidMoves(CardColumn stockPile, CardColumn stockPileDump, CardColumn[] tableauPiles,
            CardColumn[] foundationPiles);
        bool IsGameEnd(CardColumn stockPile, CardColumn stockPileDump, CardColumn[] tableauPiles,
            CardColumn[] foundationPiles, out GameOutcome gameOutcome);
    }
}