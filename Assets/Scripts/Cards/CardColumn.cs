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
        
        public readonly Stack<Card> CardStack;
        private readonly int capacity;
        private readonly float cardHeightPercentage;
        private readonly float marginTopPercentage;
        private readonly VisualElement columnContainer;
        private readonly bool setTopCardFaceUpOnRemove;

        public Card TopCard => CardStack.Count <= 0 ? null : CardStack.Peek();
        public bool IsEmpty => TopCard is null;
        public int CardCount => CardStack.Count;
        
        public bool ContainsCard(Card card) => CardStack.Contains(card);

        public CardColumn(VisualElement columnContainer, int capacity = 13,  float cardHeightPercentage = 17f, 
            float marginTopPercentage = 0f, bool setTopCardFaceUpOnRemove = true)
        {
            CardStack = new Stack<Card>(capacity);
            this.capacity = capacity;
            this.cardHeightPercentage = cardHeightPercentage;
            this.marginTopPercentage = marginTopPercentage;
            this.columnContainer = columnContainer;
            this.setTopCardFaceUpOnRemove = setTopCardFaceUpOnRemove;
                
            columnContainer.RegisterCallback<ClickEvent>(_ => CardColumnClicked?.Invoke(this));
        }
        
        public void AddCard(Card card)
        {
            if (CardStack.Count >= capacity)
            {
                Debug.LogError("Card stack is at capacity");
                return;
            }
            
            card.SetHeightPercentage(cardHeightPercentage);
            card.style.marginTop = CardStack.Count == 0 ? 0 : Length.Percent(marginTopPercentage);
            CardStack.Push(card);
            columnContainer.Add(card);
        }

        public void RemoveCard(Card card)
        {
            if (!ContainsCard(card)) throw new Exception($"{card} does not belong to this pile ({columnContainer.name})");
            if (TopCard != card) throw new Exception($"{card} cannot be removed as it is not at the top of the stack");
            
            CardStack.Pop();
            columnContainer.Remove(card);
            if (setTopCardFaceUpOnRemove) TopCard?.SetCardFace(CardFace.FaceUp);
        }
        
        public void RemoveCards(List<Card> cards)
        {
            for (var i = cards.Count - 1; i >= 0; i--)
            {
                RemoveCard(cards[i]);
            }
        }
        
        public List<Card> GetCardsAbove(Card card)
        {
            if (!ContainsCard(card)) throw new Exception($"{card} does not belong to this pile ({columnContainer.name})");

            var stackList = new List<Card>(CardStack);
            var index = stackList.IndexOf(card);
            
            var result = new List<Card>();
            for (var i = index; i >= 0; i--)
            {
                result.Add(stackList[i]);
            }

            return result;
        }
    }
}