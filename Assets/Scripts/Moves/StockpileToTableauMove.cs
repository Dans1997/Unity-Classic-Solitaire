using System.Collections.Generic;
using Cards;
using Enums;
using Interfaces;
using UnityEngine.UIElements;

namespace Moves
{
    public class StockpileToTableauMove : IGameMove
    {
        public List<Card> Cards { get; }
        public CardColumn Origin { get; }
        public CardColumn Destination { get; }
        public bool WasFaceDown { get; }
        public int Score { get; }

        public StockpileToTableauMove(List<Card> cards, CardColumn origin, CardColumn destination, bool wasFaceDown,
            int score = 5)
        {
            Cards = cards;
            Origin = origin;
            Destination = destination;
            WasFaceDown = wasFaceDown;
            Score = score;
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