using Cards;
using Enums;
using Interfaces;

namespace Moves
{
    public class DefaultMove : IGameMove
    {
        public Card Card { get; }
        public CardColumn Origin { get; }
        public CardColumn Destination { get; }
        public bool WasFaceDown { get; }
        public int Score => 0;

        public DefaultMove(Card card, CardColumn origin, CardColumn destination, bool wasFaceDown)
        {
            Card = card;
            Origin = origin;
            Destination = destination;
            WasFaceDown = wasFaceDown;
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