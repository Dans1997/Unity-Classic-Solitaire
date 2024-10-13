using System.Collections.Generic;
using Cards;

namespace Interfaces
{
    public interface ICardDragger
    {
        void StartDrag(List<Card> cards);
        void EndDrag(bool resetPosition = true);
    }
}