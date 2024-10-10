using System.Collections.Generic;
using Cards;
using Enums;
using Interfaces;

namespace Moves
{
    public class DrawFromStockpileMove : IGameMove
    {
        public List<Card> Cards { get; }
        public CardColumn Origin { get; }
        public CardColumn Destination { get; }
        public bool WasFaceDown => true;
        public int Score => 0;

        public DrawFromStockpileMove(List<Card> cards, CardColumn origin, CardColumn destination)
        {
            Cards = cards;
            Origin = origin;
            Destination = destination;
        }

        public void Execute()
        {
            foreach (var card in Cards)
            {
                Origin.RemoveCard(card);
                Destination.AddCard(card);
                card.SetCardFace(CardFace.FaceUp);
            }
        }

        public void Undo()
        {
            foreach (var card in Cards)
            {            
                Destination.RemoveCard(card);
                Origin.AddCard(card);
                card.SetCardFace(CardFace.FaceDown);
            }
        }
    }
}