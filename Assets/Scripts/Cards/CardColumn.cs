using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Cards
{
    public class CardColumn : VisualElement
    {
        private Stack<Card> cardStack;
        private VisualElement columnContainer;

        public Card TopCard => cardStack.Peek();

        public CardColumn(int initialCardCount, VisualElement columnContainer)
        {
            cardStack = new Stack<Card>(initialCardCount);
            this.columnContainer = columnContainer;
        }
        
        public void AddCard(Card card)
        {
            card.style.marginTop = cardStack.Count == 0 ? 0 : new StyleLength(-150f);
            cardStack.Push(card);
            columnContainer.Add(card);
        }

        public void RemoveCard(Card card)
        {
            // TODO: 
            cardStack.Pop();
            columnContainer.Remove(card);
        }
    }
}