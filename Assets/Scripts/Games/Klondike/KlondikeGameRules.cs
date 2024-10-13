using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Enums;
using Interfaces;
using Utils;

namespace Games.Klondike
{
    public class KlondikeGameRules : IGameRules
    {
        public int ColumnCount => 7;

        public bool CanMoveToFoundation(List<Card> cardsToMove, CardColumn foundation)
        {
            if (cardsToMove is null || cardsToMove.Count == 0) throw new ArgumentNullException(nameof(cardsToMove));
            if (foundation is null) throw new ArgumentNullException(nameof(foundation));
            if (cardsToMove.Count > 1) return false;

            var cardToMove = cardsToMove[0];
            if (foundation.IsEmpty) return cardToMove.Rank == CardRank.Ace;

            var topCard = foundation.TopCard;
            return cardToMove.Suit == topCard.Suit && cardToMove.Rank == topCard.Rank + 1;
        }

        public bool CanMoveToTableau(List<Card> cardsToMove, CardColumn tableau)
        {
            if (cardsToMove is null || cardsToMove.Count == 0) throw new ArgumentNullException(nameof(cardsToMove));
            if (tableau is null) throw new ArgumentNullException(nameof(tableau));

            var firstCard = cardsToMove[0];
            if (tableau.IsEmpty && firstCard.Rank == CardRank.King) return true;

            var topCard = tableau.TopCard;
            if (topCard == null || !CardUtils.IsOppositeColor(firstCard, topCard) || firstCard.Rank != topCard.Rank + 1) return false;

            for (var i = 1; i < cardsToMove.Count; i++)
            {
                var previousCard = cardsToMove[i - 1];
                var currentCard = cardsToMove[i];
                if (previousCard.Suit == currentCard.Suit || previousCard.Rank != currentCard.Rank + 1) return false;
            }

            return true;
        }
        
        public bool IsFoundationPileComplete(CardColumn foundationPile)
        {
            if (foundationPile is not { CardCount: 13 }) return false;

            var topCard = foundationPile.TopCard;
            return topCard is { Rank: CardRank.King };
        }
        
        public bool HasValidMoves(CardColumn stockPile, CardColumn stockPileDump, CardColumn[] tableauPiles, 
            CardColumn[] foundationPiles)
        {
            if (!stockPileDump.IsEmpty)
            {
                var stockTopCard = stockPileDump.TopCard;
                var cardsToMove = new List<Card> { stockTopCard };
                
                if (foundationPiles.Any(foundation => CanMoveToFoundation(cardsToMove, foundation)))
                {
                    return true;
                }
                
                if (tableauPiles.Any(tableau => CanMoveToTableau(cardsToMove, tableau)))
                {
                    return true;
                }
            }
            
            foreach (var tableau in tableauPiles)
            {
                var tableauTopCard = tableau.TopCard;
                if (tableau.IsEmpty) continue;
                var cardsToMove = new List<Card> { tableauTopCard };
                
                if (foundationPiles.Any(foundation => CanMoveToFoundation(cardsToMove, foundation)))
                {
                    return true;
                }
                
                if (tableauPiles.Any(otherTableau => otherTableau != tableau && CanMoveToTableau(cardsToMove, otherTableau)))
                {
                    return true;
                }
            }
            
            return !stockPile.IsEmpty;
        }

        public bool IsGameEnd(CardColumn stockPile, CardColumn stockPileDump, CardColumn[] tableauPiles,
            CardColumn[] foundationPiles, out GameOutcome gameOutcome)
        {
            var isWin = foundationPiles.All(IsFoundationPileComplete);

            if (isWin)
            {
                gameOutcome = GameOutcome.Win;
                return true;
            }
            
            var hasValidMoves = HasValidMoves(stockPile, stockPileDump, tableauPiles, foundationPiles);

            if (!hasValidMoves)
            {
                gameOutcome = GameOutcome.Lose;
                return true;
            }

            gameOutcome = (GameOutcome)(-1);
            return false;
        }
    }
}