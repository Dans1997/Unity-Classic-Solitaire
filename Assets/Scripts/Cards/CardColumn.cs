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
        
        public readonly PileType PileType;
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

        public CardColumn(PileType pileType, VisualElement columnContainer, int capacity = 13,  float cardHeightPercentage = 17f, 
            float marginTopPercentage = 0f, bool setTopCardFaceUpOnRemove = true)
        {
            PileType = pileType;
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
            if (CardStack.Count >= capacity) throw new Exception("Card stack is at capacity");
            
            card.RegisterCallback<GeometryChangedEvent>(OnCardGeometryChanged);
            card.SetHeightPercentage(cardHeightPercentage);
            CardStack.Push(card);
            columnContainer.Add(card);
        }

        public void RemoveCard(Card card)
        {
            if (!ContainsCard(card)) throw new Exception($"{card} does not belong to this pile ({columnContainer.name})");
            if (TopCard != card) throw new Exception($"{card} cannot be removed as it is not at the top of the stack");
            
            card.UnregisterCallback<GeometryChangedEvent>(OnCardGeometryChanged);
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
        
        private void OnCardGeometryChanged(GeometryChangedEvent evt)
        {
            if (evt.target is not Card card) return;
            
            // This is set to 1 because of the background element in the column
            var isFirstChild = card.parent != null && card.parent.hierarchy[1] == card;
            
            if (PileType is PileType.StockPile)
            {
                card.style.marginTop = new StyleLength(isFirstChild ? 0 : -card.resolvedStyle.height);
            }
            else
            {
                card.style.marginTop = Length.Percent(isFirstChild ? 0 : marginTopPercentage);
            }
        }
    }
}