using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cards
{
    public class CardColumn : VisualElement
    {
        public event Action<CardColumn> CardColumnClicked;
        
        private readonly Stack<Card> cardStack;
        private readonly int capacity;
        private readonly float cardHeightPercentage;
        private readonly float marginTopPercentage;
        
        private VisualElement columnContainer;
        private readonly bool setTopCardFaceUpOnRemove;

        public Card TopCard => cardStack.Count <= 0 ? null : cardStack.Peek();
        public bool IsEmpty => TopCard is null;

        public bool ContainsCard(Card card) => cardStack.Contains(card);

        public CardColumn(VisualElement columnContainer, int capacity = 13,  float cardHeightPercentage = 17f, 
            float marginTopPercentage = 0f, bool setTopCardFaceUpOnRemove = true)
        {
            cardStack = new Stack<Card>(capacity);
            this.capacity = capacity;
            this.cardHeightPercentage = cardHeightPercentage;
            this.marginTopPercentage = marginTopPercentage;
            this.columnContainer = columnContainer;
            this.setTopCardFaceUpOnRemove = setTopCardFaceUpOnRemove;
                
            columnContainer.RegisterCallback<ClickEvent>(evt => CardColumnClicked?.Invoke(this));
        }
        
        public void AddCard(Card card)
        {
            if (cardStack.Count >= capacity)
            {
                Debug.LogError("Card stack is at capacity");
                return;
            }
            
            card.SetHeightPercentage(cardHeightPercentage);
            card.style.marginTop = cardStack.Count == 0 ? 0 : Length.Percent(marginTopPercentage);
            cardStack.Push(card);
            columnContainer.Add(card);
        }

        public void RemoveCard(Card card)
        {
            if (!ContainsCard(card)) throw new Exception($"{card} does not belong to this pile ({columnContainer.name})");
            cardStack.Pop();
            columnContainer.Remove(card);
            if (setTopCardFaceUpOnRemove) TopCard?.SetCardFace(CardFace.FaceUp);
        }
    }
}