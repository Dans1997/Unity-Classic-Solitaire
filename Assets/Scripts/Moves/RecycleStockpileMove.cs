using System.Collections.Generic;
using Cards;
using Enums;
using Interfaces;

namespace Moves
{
    public class RecycleStockPileMove : IGameMove
    {
        public List<Card> Cards => null;
        public CardColumn Origin { get; }
        public CardColumn Destination { get; }
        public bool WasFaceDown => true;
        public int Score { get; }

        public RecycleStockPileMove(CardColumn origin, CardColumn destination, int score = -20)
        {
            Origin = origin;
            Destination = destination;
            Score = score;
        }

        public void Execute()
        {
            var cardsToMove = Origin.CardStack;
            while (cardsToMove.TryPeek(out var card))
            {
                Origin.RemoveCard(card);
                Destination.AddCard(card);
                card.SetCardFace(CardFace.FaceDown);
            }
        }

        public void Undo()
        {
            var cardsToMove = Destination.CardStack;
            while (cardsToMove.TryPeek(out var card))
            {
                Destination.RemoveCard(card);
                Origin.AddCard(card);
                card.SetCardFace(CardFace.FaceUp);
            }
        }
    }
}