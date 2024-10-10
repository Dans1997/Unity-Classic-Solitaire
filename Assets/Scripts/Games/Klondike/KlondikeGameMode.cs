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
using UnityEngine.UIElements;
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
        private CardColumn stockPileDump;
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

            stockPile = new CardColumn(gameplayScreen.FindColumn("stock-pile"), 24, 70f, -140f, false);
            stockPileDump = new CardColumn(gameplayScreen.FindColumn("stock-pile-dump"), 24, 70f, -140f, false);
            stockPile.CardColumnClicked += OnStockPileClicked;

            foundationPiles = new[]
            {
                new CardColumn(gameplayScreen.FindColumn("foundations-0"), 13, 70f, -140f),
                new CardColumn(gameplayScreen.FindColumn("foundations-1"), 13, 70f, -140f),
                new CardColumn(gameplayScreen.FindColumn("foundations-2"), 13, 70f, -140f),
                new CardColumn(gameplayScreen.FindColumn("foundations-3"), 13, 70f, -140f),
            };

            foreach (var foundationPile in foundationPiles)
            {
                foundationPile.CardColumnClicked += OnFoundationPileClicked;
            }
            
            tableauPiles = new []
            {
                new CardColumn(gameplayScreen.FindColumn("card-column-0"), cardHeightPercentage: 17f, marginTopPercentage: -100f),
                new CardColumn(gameplayScreen.FindColumn("card-column-1"), cardHeightPercentage: 17f, marginTopPercentage: -100f),
                new CardColumn(gameplayScreen.FindColumn("card-column-2"), cardHeightPercentage: 17f, marginTopPercentage: -100f),
                new CardColumn(gameplayScreen.FindColumn("card-column-3"), cardHeightPercentage: 17f, marginTopPercentage: -100f),
                new CardColumn(gameplayScreen.FindColumn("card-column-4"), cardHeightPercentage: 17f, marginTopPercentage: -100f),
                new CardColumn(gameplayScreen.FindColumn("card-column-5"), cardHeightPercentage: 17f, marginTopPercentage: -100f),
                new CardColumn(gameplayScreen.FindColumn("card-column-6"), cardHeightPercentage: 17f, marginTopPercentage: -100f)
            };
            
            var deck = Enum.GetValues(typeof(CardType)).Cast<CardType>().ToList();
            var rng = new Random();
            var shuffledDeck = deck.OrderBy(_ => rng.Next()).ToList();
            var columnsCount = 7;
            for (var columnIndex = 0; columnIndex < columnsCount; columnIndex++)
            {
                for (var cardIndex = 0; cardIndex <= columnIndex; cardIndex++)
                {
                    var card = new Card(shuffledDeck[0], Constants.ClassicSolitaireFaceDownSpriteKey);
                    yield return card.LoadCardSprites();

                    card.CardClicked += OnCardClicked;
                    shuffledDeck.RemoveAt(0);
                    tableauPiles[columnIndex].AddCard(card);
                }
            }

            var stockPileCount = shuffledDeck.Count;
            for (var i = 0; i < stockPileCount; i++)
            {
                var card = new Card(shuffledDeck[0], Constants.ClassicSolitaireFaceDownSpriteKey);
                yield return card.LoadCardSprites();
                
                card.CardClicked += OnCardClicked;
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

                if (!isFoundationPile && !isTableauPile)
                {
                    Debug.Log($"Cannot move {selectedCard} to this pile because it's not a tableau or a foundation");
                    ResetSelection();
                    return;
                }

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
                selectedCard.SetHeightPercentage(75f);
                selectedCard.pickingMode = PickingMode.Ignore;
                ResetSelection();
                return;
            }

            selectedCard ??= clickedCard;
            Debug.Log($"{selectedCard} was selected");
        }
        
        private void OnFoundationPileClicked(CardColumn clickedFoundationPile)
        {
            if (selectedCard is null)
            {
                Debug.Log($"{clickedFoundationPile} was clicked, but no card is selected");
                return;
            }

            if (!foundationPiles.Contains(clickedFoundationPile))
            {
                Debug.LogError($"{clickedFoundationPile} is not a foundation pile");
                return;
            }
            
            var originColumn = GetCardColumn(selectedCard);
            RegisterMove(new Move(selectedCard, originColumn, clickedFoundationPile, selectedCard.CardFace == CardFace.FaceDown));
            originColumn.RemoveCard(selectedCard);
            clickedFoundationPile.AddCard(selectedCard);
            ResetSelection();
        }
        
        private void OnStockPileClicked(CardColumn clickedStockPile)
        {
            if (stockPile != clickedStockPile)
            {
                Debug.LogError($"{clickedStockPile} is not a stock pile");
                return;
            }

            if (stockPile.IsEmpty)
            {
                Debug.Log("Resetting stock pile");
                // TODO: Move all cards from the dump back to the stock pile
                return;
            }

            if (selectedCard is not null)
            {
                Debug.Log("Clicked on stock pile with a selected card");
                ResetSelection();
                return;
            }

            var cardToMove = stockPile.TopCard;
            cardToMove.SetCardFace(CardFace.FaceUp);
            RegisterMove(new Move(cardToMove, stockPile, stockPileDump, cardToMove.CardFace == CardFace.FaceDown));
            stockPile.RemoveCard(cardToMove);
            stockPileDump.AddCard(cardToMove);
            ResetSelection();
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
            if (stockPileDump.ContainsCard(card)) return stockPileDump;
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