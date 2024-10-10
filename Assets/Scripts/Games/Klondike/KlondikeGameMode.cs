using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Enums;
using Interfaces;
using Moves;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;
using Random = System.Random;

namespace Games.Klondike
{
    public class KlondikeGameMode : IGameMode
    {
        public GameMode GameMode => GameMode.Klondike;
        
        private readonly IGameRules gameRules;
        private readonly Stack<Move> moves;
        private readonly string gameplayScreenPrefabKey;
        private IGameplayScreen gameplayScreen;
        private CardColumn stockPile;
        private CardColumn[] foundationPiles;
        private CardColumn[] tableauPiles;
        private Card[] cards;

        private Card selectedCard;

        public KlondikeGameMode(string gameplayScreenPrefabKey)
        {
            gameRules = new KlondikeGameRules();
            moves = new Stack<Move>();
            this.gameplayScreenPrefabKey = gameplayScreenPrefabKey;
        }
        
        public IEnumerator InitializeGame()
        {
            yield return CreateGameplayScreen();
            
            gameplayScreen.SettingsButtonClicked += OnOptionsButtonClicked;
            gameplayScreen.ExitButtonClicked += OnExitButtonClicked;
            gameplayScreen.PauseButtonClicked += OnPauseButtonClicked;
            gameplayScreen.UndoButtonClicked += OnUndoMoveClicked;

            yield return gameplayScreen.Show();

            stockPile = new CardColumn(gameplayScreen.FindColumn("stock-pile"), 24, -150f);

            foundationPiles = new[]
            {
                new CardColumn(gameplayScreen.FindColumn("foundations-0"), 13, -150f),
                new CardColumn(gameplayScreen.FindColumn("foundations-1"), 13, -150f),
                new CardColumn(gameplayScreen.FindColumn("foundations-2"), 13, -150f),
                new CardColumn(gameplayScreen.FindColumn("foundations-3"), 13, -150f),
            };
            
            tableauPiles = new []
            {
                new CardColumn(gameplayScreen.FindColumn("card-column-0"), marginTopPercentage: -100f),
                new CardColumn(gameplayScreen.FindColumn("card-column-1"), marginTopPercentage: -100f),
                new CardColumn(gameplayScreen.FindColumn("card-column-2"), marginTopPercentage: -100f),
                new CardColumn(gameplayScreen.FindColumn("card-column-3"), marginTopPercentage: -100f),
                new CardColumn(gameplayScreen.FindColumn("card-column-4"), marginTopPercentage: -100f),
                new CardColumn(gameplayScreen.FindColumn("card-column-5"), marginTopPercentage: -100f),
                new CardColumn(gameplayScreen.FindColumn("card-column-6"), marginTopPercentage: -100f)
            };
            
            var deck = Enum.GetValues(typeof(CardType)).Cast<CardType>().ToList();
            var rng = new Random();
            var shuffledDeck = deck.OrderBy(_ => rng.Next()).ToList();
            var columnsCount = 7;
            for (var columnIndex = 0; columnIndex < columnsCount; columnIndex++)
            {
                for (var cardIndex = 0; cardIndex <= columnIndex; cardIndex++)
                {
                    var card = new Card(shuffledDeck[0], Constants.ClassicSolitaireFaceDownSpriteKey, 17f);
                    yield return card.LoadCardSprites();

                    card.CardClicked += OnCardClicked;
                    shuffledDeck.RemoveAt(0);
                    tableauPiles[columnIndex].AddCard(card);
                }
            }

            var stockPileCount = shuffledDeck.Count;
            for (var i = 0; i < stockPileCount; i++)
            {
                var card = new Card(shuffledDeck[0], Constants.ClassicSolitaireFaceDownSpriteKey, 75f);
                yield return card.LoadCardSprites();
                
                shuffledDeck.RemoveAt(0);
                stockPile.AddCard(card);
                card.SetCardFace(CardFace.FaceDown);
            }

            foreach (var cardColumn in tableauPiles)
            {
                cardColumn.TopCard.SetCardFace(CardFace.FaceUp);
            }
        }

        public IEnumerator RunGame()
        {
            yield break;
        }
        
        private void OnCardClicked(Card clickedCard)
        {
            if (clickedCard.CardFace == CardFace.FaceDown) return;

            if (selectedCard is not null)
            {
                var originColumn = GetCardColumn(selectedCard);
                var destinationColumn = GetCardColumn(clickedCard);
                var isFoundationPile = foundationPiles.Contains(destinationColumn);
                var isTableauPile = tableauPiles.Contains(destinationColumn);

                if (isFoundationPile && !gameRules.CanMoveToFoundation(selectedCard, destinationColumn))
                {
                    Debug.Log($"Cannot move {selectedCard} to foundation pile with top card {destinationColumn.TopCard}");
                    ResetSelection();
                    return;
                }

                if (isTableauPile && !gameRules.CanMoveToTableau(selectedCard, destinationColumn))
                {
                    Debug.Log($"Cannot move {selectedCard} to tableau pile with top card {destinationColumn.TopCard}");
                    ResetSelection();
                    return;
                }
                
                RegisterMove(new Move(selectedCard, originColumn, destinationColumn, clickedCard.CardFace == CardFace.FaceDown));
                originColumn.RemoveCard(selectedCard);
                destinationColumn.AddCard(selectedCard);
                ResetSelection();
                return;
            }

            selectedCard ??= clickedCard;
            Debug.Log($"{selectedCard} was selected");
        }

        private IEnumerator CreateGameplayScreen()
        {
            var handle = Addressables.InstantiateAsync(gameplayScreenPrefabKey);
            yield return handle;
            gameplayScreen = handle.Result.GetComponent<IGameplayScreen>();
        }
        
        private void OnOptionsButtonClicked()
        {
            throw new System.NotImplementedException();
        }

        private void OnExitButtonClicked()
        {
            throw new System.NotImplementedException();
        }

        private void OnPauseButtonClicked()
        {
            throw new System.NotImplementedException();
        }

        private void OnUndoMoveClicked()
        {
            if (moves.Count <= 0) return;
            
            var lastMove = moves.Pop();
            lastMove.Undo();
        }

        private CardColumn GetCardColumn(Card card)
        {
            if (stockPile.ContainsCard(card)) return stockPile;
            foreach (var foundation in foundationPiles) if (foundation.ContainsCard(card)) return foundation;
            foreach (var column in tableauPiles) if (column.ContainsCard(card)) return column;

            throw new Exception($"{card} was not found in any pile");
        }

        private void RegisterMove(Move move)
        {
            moves.Push(move);
        }
        
        private void ResetSelection()
        {
            selectedCard = null;
        }
    }
}