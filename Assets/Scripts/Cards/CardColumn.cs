using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cards
{
    public class CardColumn : VisualElement
    {
        private readonly Stack<Card> cardStack;
        private readonly int capacity;
        private readonly float marginTopPercentage;
        
        private VisualElement columnContainer;

        public Card TopCard => cardStack.Count <= 0 ? null : cardStack.Peek();
        public bool ContainsCard(Card card) => cardStack.Contains(card);

        public CardColumn(VisualElement columnContainer, int capacity = 13, float marginTopPercentage = 0f)
        {
            cardStack = new Stack<Card>(capacity);
            this.capacity = capacity;
            this.marginTopPercentage = marginTopPercentage;
            this.columnContainer = columnContainer;
        }
        
        public void AddCard(Card card)
        {
            if (cardStack.Count >= capacity)
            {
                Debug.LogError("Card stack is at capacity");
                return;
            }
            
            card.style.marginTop = cardStack.Count == 0 ? 0 : Length.Percent(marginTopPercentage);
            cardStack.Push(card);
            columnContainer.Add(card);
        }

        public void RemoveCard(Card card)
        {
            if (!ContainsCard(card)) throw new Exception($"{card} does not belong to this pile ({columnContainer.name})");
            cardStack.Pop();
            columnContainer.Remove(card);
            TopCard.SetCardFace(CardFace.FaceUp);
        }
    }
}