using System.Collections.Generic;
using Cards;
using Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardDraggers
{
    public class CardDragger : ICardDragger
    {
        private readonly VisualElement gameBoard;
        private readonly VisualElement tableauTemplate;
        private readonly VisualElement columnElement;
        private List<Card> draggedCards;
        private Vector2 offset;
        private VisualElement originalParent;

        private Vector2 MousePosition => gameBoard.WorldToLocal(Input.mousePosition);

        public CardDragger(IScreen screen)
        {
            var root = screen.UIDocument.rootVisualElement;
            gameBoard = root.Q<VisualElement>("game-board");
            tableauTemplate = root.Q<VisualElement>("card-column-0"); // Could be any tableau pile
            
            var overlay = new VisualElement
            { 
                style = 
                { 
                    position = Position.Absolute, 
                    top = 0, 
                    left = 0, 
                    right = 0, 
                    bottom = 0 
                },
                pickingMode = PickingMode.Ignore
            };
            
            columnElement = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    height = tableauTemplate.resolvedStyle.height,
                    width = tableauTemplate.resolvedStyle.width,
                },
                pickingMode = PickingMode.Ignore
            };
            
            columnElement.AddToClassList("column");
            columnElement.AddToClassList("overlay-card-column");
            overlay.Add(columnElement);
            gameBoard.Add(overlay);
            
            root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        }

        public void StartDrag(List<Card> cards)
        {
            var firstCard = cards[0];
            originalParent = firstCard.parent;
            
            draggedCards = cards;
            foreach (var card in cards)
            {
                card.pickingMode = PickingMode.Ignore;
                columnElement.Add(card);
            }
            
            UpdateColumnPosition();
        }
        
        public void EndDrag(bool resetPosition = true)
        {
            if (draggedCards is null) return;
            
            foreach (var card in draggedCards)
            {
                card.pickingMode = PickingMode.Position;
                if (resetPosition) originalParent.Add(card);
            }

            draggedCards = null;
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (draggedCards == null) return;

            UpdateColumnPosition();
        }

        private void UpdateColumnPosition()
        {
            columnElement.style.height = tableauTemplate.resolvedStyle.height;
            columnElement.style.width = tableauTemplate.resolvedStyle.width;
            columnElement.style.left = MousePosition.x - columnElement.resolvedStyle.width / 2;
            columnElement.style.bottom = MousePosition.y - columnElement.resolvedStyle.height;
        }
    }
}