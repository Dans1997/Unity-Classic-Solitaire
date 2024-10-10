using Cards;
using Enums;
using Interfaces;
using UnityEngine.UIElements;

namespace Moves
{
    public class StockpileToTableauMove : IGameMove
    {
        public Card Card { get; }
        public CardColumn Origin { get; }
        public CardColumn Destination { get; }
        public bool WasFaceDown { get; }
        public int Score { get; }

        public StockpileToTableauMove(Card card, CardColumn origin, CardColumn destination, bool wasFaceDown,
            int score = 5)
        {
            Card = card;
            Origin = origin;
            Destination = destination;
            WasFaceDown = wasFaceDown;
            Score = score;
        }

        public void Execute()
        {
            Origin.RemoveCard(Card);
            Destination.AddCard(Card);
        }

        public void Undo()
        {
            Destination.RemoveCard(Card);
            Origin.AddCard(Card);
            if (WasFaceDown) Card.SetCardFace(CardFace.FaceDown);
        }
    }
}