using Cards;
using Enums;
using Interfaces;

namespace Moves
{
    public class DrawFromStockpileMove : IGameMove
    {
        public Card Card { get; }
        public CardColumn Origin { get; }
        public CardColumn Destination { get; }
        public bool WasFaceDown => true;
        public int Score => 0;

        public DrawFromStockpileMove(Card card, CardColumn origin, CardColumn destination)
        {
            Card = card;
            Origin = origin;
            Destination = destination;
        }

        public void Execute()
        {
            Origin.RemoveCard(Card);
            Destination.AddCard(Card);
            Card.SetCardFace(CardFace.FaceUp);
        }

        public void Undo()
        {
            Destination.RemoveCard(Card);
            Origin.AddCard(Card);
            Card.SetCardFace(CardFace.FaceDown);
        }
    }
}