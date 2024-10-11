using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public event Action PauseButtonClicked;
        public event Action GameFinished;
        public GameMode GameMode => GameMode.Klondike;
        
        private readonly IGameRules gameRules;
        private readonly Stack<IGameMove> moves;
        private readonly ITimer timer;
        private readonly IScreen pauseScreen;
        private readonly string gameplayScreenPrefabKey;
        private IGameplayScreen gameplayScreen;
        private CardColumn stockPile;
        private CardColumn stockPileDump;
        private CardColumn[] foundationPiles;
        private CardColumn[] tableauPiles;
        private Card[] cards;
        private int score;
        private float gameStartTime;
        private List<Card> selectedCards;

        public KlondikeGameMode(ITimer timer, string gameplayScreenPrefabKey)
        {
            this.timer = timer;
            this.gameplayScreenPrefabKey = gameplayScreenPrefabKey;
            
            gameRules = new KlondikeGameRules();
            moves = new Stack<IGameMove>();
        }
        

        public IEnumerator DealCards()
        {
            yield return CreateGameplayScreen();
            
            gameplayScreen.SettingsButtonClicked += OnOptionsButtonClicked;
            gameplayScreen.ExitButtonClicked += OnExitButtonClicked;
            gameplayScreen.PauseButtonClicked += () => PauseButtonClicked?.Invoke();
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
            
            var faceDownSpriteHandle = Addressables.LoadAssetAsync<Sprite>(Constants.ClassicSolitaireFaceDownSpriteKey);
            yield return faceDownSpriteHandle;
            
            var deck = Enum.GetValues(typeof(CardType)).Cast<CardType>().ToList();
            var rng = new Random();
            var shuffledDeck = deck.OrderBy(_ => rng.Next()).ToList();
            var columnsCount = 7;
            for (var columnIndex = 0; columnIndex < columnsCount; columnIndex++)
            {
                for (var cardIndex = 0; cardIndex <= columnIndex; cardIndex++)
                {
                    var card = new Card(shuffledDeck[0], faceDownSpriteHandle.Result);
                    yield return card.LoadCardSprites();

                    card.CardClicked += SelectCardColumn;
                    card.CardFaceChanged += OnCardFaceChanged;
                    shuffledDeck.RemoveAt(0);
                    tableauPiles[columnIndex].AddCard(card);
                }
            }

            var stockPileCount = shuffledDeck.Count;
            for (var i = 0; i < stockPileCount; i++)
            {
                var card = new Card(shuffledDeck[0], faceDownSpriteHandle.Result);
                yield return card.LoadCardSprites();
                
                card.CardClicked += SelectCardColumn;
                shuffledDeck.RemoveAt(0);
                stockPile.AddCard(card);
                card.SetCardFace(CardFace.FaceDown);
            }

            foreach (var cardColumn in tableauPiles)
            {
                cardColumn.TopCard.SetCardFace(CardFace.FaceUp);
            }
        }

        public void StartGame()
        {
            if (score != 0) throw new Exception("Game's score is dirty");
            if (gameStartTime != 0) throw new Exception("Game's timeSinceStart is dirty");
            if (moves.Count != 0) throw new Exception("Game's moves are dirty");
            
            gameStartTime = Time.time;
            timer.StartTimer(() => gameplayScreen.SetTime(timer.ElapsedTime), 1f);
        }

        private void OnFoundationPileClicked(CardColumn clickedFoundationPile)
        {
            if (selectedCards is null)
            {
                Debug.Log($"{clickedFoundationPile} was clicked, but no card is selected");
                return;
            }

            if (!foundationPiles.Contains(clickedFoundationPile))
            {
                Debug.LogError($"{clickedFoundationPile} is not a foundation pile");
                return;
            }
            
            if (TryCreateMove(clickedFoundationPile, out var move)) RegisterMove(move);
            ResetSelection();
        }
        
        private void OnTableauPileClicked(CardColumn clickedTableauPile)
        {
            if (selectedCards is null)
            {
                Debug.Log($"{clickedTableauPile} was clicked, but no card is selected");
                return;
            }
            
            if (!tableauPiles.Contains(clickedTableauPile))
            {
                Debug.LogError($"{clickedTableauPile} is not a tableau pile");
                return;
            }
            
            if (TryCreateMove(clickedTableauPile, out var move)) RegisterMove(move);
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
                if (stockPileDump.IsEmpty) return;
                
                RegisterMove(new RecycleStockPileMove(stockPileDump, stockPile));
                return;
            }

            if (selectedCards is not null)
            {
                Debug.Log("Clicked on stock pile with a selected card");
                ResetSelection();
                return;
            }

            var cardToMove = stockPile.TopCard;
            RegisterMove(new DrawFromStockpileMove(new List<Card> { cardToMove }, stockPile, stockPileDump));
        }

        private bool TryCreateMove(CardColumn destinationColumn, out IGameMove move)
        {
            move = null;
            
            if (selectedCards.Count <= 0) return false;
            if (selectedCards is null) throw new Exception("No card selected to create a move");
            if (destinationColumn is null) throw new Exception("No destination selected to create a move");
            
            var originColumn = GetCardOriginPile(selectedCards[0]);
            var isOriginStockpile = stockPileDump == originColumn;
            var isOriginTableau = tableauPiles.Contains(originColumn);
            var isDestinationFoundation = foundationPiles.Contains(destinationColumn);
            var isDestinationTableau = tableauPiles.Contains(destinationColumn);

            if (isOriginStockpile && isOriginTableau)
            {
                Debug.LogError("Origin pile cannot be both a stock pile and a tableau pile at the same time");
                return false;
            }
            
            if (!isOriginStockpile && !isOriginTableau)
            {
                Debug.Log($"Cannot move {selectedCards[0]} because its origin is not a tableau or a stock pile");
                return false;
            }
            
            if (isDestinationFoundation && isDestinationTableau)
            {
                Debug.LogError($"{destinationColumn} cannot be both a foundation and a tableau pile");
                return false;
            }

            if (!isDestinationFoundation && !isDestinationTableau)
            {
                Debug.Log($"Cannot move {selectedCards[0]} to this pile because it's not a tableau or a foundation");
                return false;
            }

            if (isDestinationFoundation && !gameRules.CanMoveToFoundation(selectedCards[0], destinationColumn))
            {
                Debug.Log($"Cannot move {selectedCards[0]} to foundation pile with top card {destinationColumn.TopCard}");
                return false;
            }

            if (isDestinationTableau && !gameRules.CanMoveToTableau(selectedCards[0], destinationColumn))
            {
                Debug.Log($"Cannot move {selectedCards[0]} to tableau pile with top card {destinationColumn.TopCard}");
                return false;
            }

            if (isOriginStockpile && isDestinationTableau)
            {
                move = new StockpileToTableauMove(selectedCards, originColumn, destinationColumn, 
                    selectedCards[0].CardFace == CardFace.FaceDown);
            }
            else if (isOriginStockpile)
            {
                move = new StockpileToFoundationMove(selectedCards, originColumn, destinationColumn, 
                    selectedCards[0].CardFace == CardFace.FaceDown);
            }
            else if (isDestinationFoundation)
            {
                move = new TableauToFoundationMove(selectedCards, originColumn, destinationColumn, 
                    selectedCards[0].CardFace == CardFace.FaceDown);
            }
            else
            {
                move = new DefaultMove(selectedCards, originColumn, destinationColumn, 
                    selectedCards[0].CardFace == CardFace.FaceDown);
            }

            return true;
        }
        
        private void OnCardFaceChanged(CardFace oldFace, CardFace newFace)
        {
            if (gameStartTime == 0) return;

            switch (oldFace)
            {
                case CardFace.FaceDown when newFace == CardFace.FaceUp:
                    AddScore(5);
                    break;
                case CardFace.FaceUp when newFace == CardFace.FaceDown:
                    AddScore(-5);
                    break;
            }
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

        private void OnUndoMoveClicked()
        {
            if (moves.Count <= 0) return;
            UndoLastMove();
        }

        private CardColumn GetCardOriginPile(Card card)
        {
            if (stockPile.ContainsCard(card)) return stockPile;
            if (stockPileDump.ContainsCard(card)) return stockPileDump;
            foreach (var foundation in foundationPiles) if (foundation.ContainsCard(card)) return foundation;
            foreach (var column in tableauPiles) if (column.ContainsCard(card)) return column;

            throw new Exception($"{card} was not found in any pile");
        }

        private void RegisterMove(IGameMove move)
        {
            moves.Push(move);
            move.Execute();
            AddScore(move.Score);
            gameplayScreen.SetMoveCount(moves.Count);
        }
        
        private void UndoLastMove()
        {
            var move = moves.Pop();
            move.Undo();
            AddScore(-move.Score);
            gameplayScreen.SetMoveCount(moves.Count);
        }
        
        private async void SelectCardColumn(Card clickedCard)
        {
            if (clickedCard.CardFace == CardFace.FaceDown) return;
            if (selectedCards is not null) return;

            var cardOriginPile = GetCardOriginPile(clickedCard);
            selectedCards = cardOriginPile.GetCardsAbove(clickedCard);

            foreach (var card in selectedCards)
            {
                card.SetSelected(true);
            }

            await Task.Yield();
            foreach (var tableauPile in tableauPiles)
            {
                tableauPile.CardColumnClicked += OnTableauPileClicked;
            }
        }
        
        private void ResetSelection()
        {
            foreach (var card in selectedCards)
            {
                card.SetSelected(false);
            }
            
            selectedCards = null;
            
            foreach (var tableauPile in tableauPiles)
            {
                tableauPile.CardColumnClicked -= OnTableauPileClicked;
            }
        }

        private void AddScore(int scoreToAdd)
        {
            score += scoreToAdd;
            gameplayScreen.SetScore(score);
        }
    }
}