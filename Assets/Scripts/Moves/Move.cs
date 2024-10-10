using Cards;
using Enums;
using Utils;

namespace Moves
{
    public class Move
    {
        public Card Card { get; }
        public CardColumn Origin { get; }
        public CardColumn Destination { get; }
        public bool WasFaceDown { get; }

        public Move(Card card, CardColumn origin, CardColumn destination, bool wasFaceDown)
        {
            Card = card;
            Origin = origin;
            Destination = destination;
            WasFaceDown = wasFaceDown;
        }

        public void Undo()
        {
            Destination.RemoveCard(Card);
            if (WasFaceDown) Card.SetCardFace(CardFace.FaceDown);
            Origin.AddCard(Card);
        }
    }
}