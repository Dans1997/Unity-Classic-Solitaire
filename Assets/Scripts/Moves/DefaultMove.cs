using System.Collections.Generic;
using Cards;
using Enums;
using Interfaces;

namespace Moves
{
    public class DefaultMove : IGameMove
    {
        public List<Card> Cards { get; }
        public CardColumn Origin { get; }
        public CardColumn Destination { get; }
        public bool WasFaceDown { get; }
        public int Score => 0;

        public DefaultMove(List<Card> cards, CardColumn origin, CardColumn destination, bool wasFaceDown)
        {
            Cards = cards;
            Origin = origin;
            Destination = destination;
            WasFaceDown = wasFaceDown;
        }

        public void Execute()
        {
            Origin.RemoveCards(Cards);
            
            foreach (var card in Cards)
            {
                Destination.AddCard(card);
            }
        }

        public void Undo()
        {
            Destination.RemoveCards(Cards);
            foreach (var card in Cards)
            {
                Origin.AddCard(card);
                if (WasFaceDown) card.SetCardFace(CardFace.FaceDown);
            }
        }
    }
}