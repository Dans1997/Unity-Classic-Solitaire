using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine.AddressableAssets;

namespace Games.Modes
{
    public class ClassicSolitaireGameMode : IGameMode
    {
        private readonly Stack<string> moves;
        private readonly string gameplayScreenPrefabKey;
        private IGameplayScreen gameplayScreen;

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
        }

        public IEnumerator RunGame()
        {
            yield break;
        }

        public void RegisterMove(string move)
        {
            moves.Push(move);
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