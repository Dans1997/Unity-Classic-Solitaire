using System.Collections.Generic;
using Cards;
using Enums;
using Interfaces;
using UnityEngine.UIElements;

namespace Moves
{
    public class StockpileToFoundationMove : IGameMove
    {
        public List<Card> Cards { get; }
        public CardColumn Origin { get; }
        public CardColumn Destination { get; }
        public bool WasFaceDown { get; }
        public int Score { get; }

        public StockpileToFoundationMove(List<Card> cards, CardColumn origin, CardColumn destination, bool wasFaceDown,
            int score = 10)
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
                card.pickingMode = PickingMode.Ignore;
            }
        }

        public void Undo()
        {
            Destination.RemoveCards(Cards);
            foreach (var card in Cards)
            {
                Origin.AddCard(card);
                if (WasFaceDown) card.SetCardFace(CardFace.FaceDown);
                card.pickingMode = PickingMode.Position;
            }
        }
    }
}