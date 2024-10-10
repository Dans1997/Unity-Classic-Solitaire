using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;
using Random = System.Random;

namespace Games.Modes
{
    public class ClassicSolitaireGameMode : IGameMode
    {
        private readonly Stack<string> moves;
        private readonly string gameplayScreenPrefabKey;
        private IGameplayScreen gameplayScreen;
        private CardColumn stockPile;
        private CardColumn[] foundations;
        private CardColumn[] cardColumns;
        private Card[] cards;

        public ClassicSolitaireGameMode(string gameplayScreenPrefabKey)
        {
            moves = new Stack<string>();
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

            foundations = new[]
            {
                new CardColumn(gameplayScreen.FindColumn("foundations-0"), 13, -150f),
                new CardColumn(gameplayScreen.FindColumn("foundations-1"), 13, -150f),
                new CardColumn(gameplayScreen.FindColumn("foundations-2"), 13, -150f),
                new CardColumn(gameplayScreen.FindColumn("foundations-3"), 13, -150f),
            };
            
            cardColumns = new []
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
                    cardColumns[columnIndex].AddCard(card);
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

            foreach (var cardColumn in cardColumns)
            {
                cardColumn.TopCard.SetCardFace(CardFace.FaceUp);
            }
        }

        public IEnumerator RunGame()
        {
            yield break;
        }

        public void RegisterMove(string move)
        {
            moves.Push(move);
        }
        
        private void OnCardClicked(Card clickedCard)
        {
            if (clickedCard.CardFace == CardFace.FaceDown) return;
            
            Debug.Log($"{clickedCard.CardType} was clicked");
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
            throw new System.NotImplementedException();
        }
        
        private void UndoLastMove()
        {
            if (moves.Count > 0)
            {
                moves.Pop();
            }
        }
    }
}