using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Cards
{
    public class CardColumn : VisualElement
    {
        private readonly Stack<Card> cardStack;
        private readonly int capacity;
        private readonly float marginTopPercentage;
        
        private VisualElement columnContainer;

        public Card TopCard => cardStack.Peek();

        public CardColumn(VisualElement columnContainer, int capacity = 13, float marginTopPercentage = 0f)
        {
            cardStack = new Stack<Card>(capacity);
            this.capacity = capacity;
            this.marginTopPercentage = marginTopPercentage;
            this.columnContainer = columnContainer;
        }
        
        public void AddCard(Card card)
        {
            card.style.marginTop = cardStack.Count == 0 ? 0 : Length.Percent(marginTopPercentage);
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