using System;
using System.Linq;
using Cards;
using Enums;
using Interfaces;
using Utils;

namespace Games.Klondike
{
    public class KlondikeGameRules : IGameRules
    {
        public bool CheckForWin(CardColumn[] foundationPiles)
        {
            return foundationPiles.All(IsFoundationPileComplete);
        }
        
        public bool CanMoveToFoundation(Card cardToMove, CardColumn foundation)
        {
            if (cardToMove is null) throw new ArgumentNullException(nameof(cardToMove));
            if (foundation is null) throw new ArgumentNullException(nameof(foundation));
            if (foundation.TopCard == null) return cardToMove.Rank == CardRank.Ace;
            
            var topCard = foundation.TopCard;
            return cardToMove.Suit == topCard.Suit && cardToMove.Rank == topCard.Rank + 1;
        }

        public bool CanMoveToTableau(Card cardToMove, CardColumn tableau)
        {
            if (cardToMove is null) throw new ArgumentNullException(nameof(cardToMove));
            if (tableau is null) throw new ArgumentNullException(nameof(tableau));
            if (tableau.TopCard == null && cardToMove.Rank == CardRank.King) return true;

            var topCard = tableau.TopCard;
            return topCard != null && CardUtils.IsOppositeColor(cardToMove, topCard) && cardToMove.Rank == topCard.Rank + 1;
        }
        
        public bool IsFoundationPileComplete(CardColumn foundationPile)
        {
            if (foundationPile is not { CardCount: 13 }) return false;

            var topCard = foundationPile.TopCard;
            return topCard is { Rank: CardRank.King };
        }
    }
}