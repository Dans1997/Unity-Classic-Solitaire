using Cards;

namespace Interfaces
{
    public interface IGameRules
    {
        bool CanMoveToFoundation(Card cardToMove, CardColumn foundation);
        bool CanMoveToTableau(Card cardToMove, CardColumn tableau);
        bool IsFoundationPileComplete(CardColumn foundationPile);
        bool CheckForWin(CardColumn[] foundationPiles);
    }
}