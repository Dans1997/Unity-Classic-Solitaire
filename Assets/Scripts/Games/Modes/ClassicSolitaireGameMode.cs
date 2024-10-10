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
            
            cardColumns = new []
            {
                new CardColumn(1, gameplayScreen.FindColumn("card-column-0")),
                new CardColumn(2, gameplayScreen.FindColumn("card-column-1")),
                new CardColumn(3, gameplayScreen.FindColumn("card-column-2")),
                new CardColumn(4, gameplayScreen.FindColumn("card-column-3")),
                new CardColumn(5, gameplayScreen.FindColumn("card-column-4")),
                new CardColumn(6, gameplayScreen.FindColumn("card-column-5")),
                new CardColumn(7, gameplayScreen.FindColumn("card-column-6"))
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
                    cardColumns[columnIndex].AddCard(card);
                }
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