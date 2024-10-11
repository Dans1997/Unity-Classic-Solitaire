using System;
using System.Collections;
using GameAnalytics;
using Games.Klondike;
using Interfaces;
using Scenes;
using Screens;
using Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Containers
{
    public class GameStateContainer : MonoBehaviour
    {
        private SceneLoader sceneLoader;
        private IUserDataTracker dataTracker;
        private IScreen loadingScreen;
        private IScreen mainMenuScreen;
        private IScreen pauseScreen;
        private IGameResultsScreen endCardScreen;

        private void Awake()
        {
            sceneLoader = new SceneLoader(SceneManager.GetActiveScene());
            dataTracker = new UserDataTracker();
        }

        private IEnumerator Start()
        {
            yield return AddressablesUtils.InitializeAddressables();
            
            yield return AddressablesUtils.CreateScreen<LoadingScreen>(Constants.LoadingScreenPrefabKey, screen =>
            {
                loadingScreen = screen;
            });
            
            yield return AddressablesUtils.CreateScreen<MainMenuScreen>(Constants.MainMenuScreenPrefabKey, screen =>
            {
                screen.PlayButtonClicked += OnPlayButtonClicked;
                screen.OptionsButtonClicked += OnOptionsButtonClicked;
                screen.ExitButtonClicked += OnExitButtonClicked;
                mainMenuScreen = screen;
            });
            
            yield return AddressablesUtils.CreateScreen<PauseScreen>(Constants.PauseScreenPrefabKey, screen => 
            {
                screen.ResumeButtonClicked += OnResumeButtonClicked;
                pauseScreen = screen;
            });
            
            yield return AddressablesUtils.CreateScreen<EndCardScreen>(Constants.EndCardScreenPrefabKey, screen => 
            {
                screen.ContinueButtonClicked += OnEndCardContinueButtonClicked;
                endCardScreen = screen;
            });

            yield return endCardScreen.Hide();
            yield return pauseScreen.Hide();
            yield return mainMenuScreen.Show();
            yield return loadingScreen.Hide();
        }

        private IEnumerator LoadSelectedGameMode(GameMode gameMode)
        {
            yield return loadingScreen.Show();
            yield return mainMenuScreen.Hide();
            yield return sceneLoader.LoadSceneAsync(Constants.GameplaySceneKey);
            
            IGameMode gameModeController = gameMode switch
            {
                GameMode.Klondike => new KlondikeGameMode(new CoroutineTimer(this), 
                    Constants.ClassicSolitaireScreenPrefabKey),
                _ => throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null)
            };
            
            yield return gameModeController.DealCards();
            yield return loadingScreen.Hide();

            gameModeController.PauseButtonClicked += OnPauseButtonClicked;
            gameModeController.MoveRegistered += OnMoveRegistered;
            gameModeController.MoveUndone += OnMoveUndone;
            gameModeController.GameFinished += OnGameFinished;
            
            var gameStartTime = gameModeController.StartGame();
            dataTracker.TrackGameStart(gameStartTime);
        }

        private IEnumerator UnloadSelectedGameMode(IGameResults gameResults)
        {
            Debug.Log($"Game finished with results: {gameResults}");
            
            endCardScreen.SetGameResultLabel($"You {gameResults.GameOutcome}!");
            endCardScreen.SetScoreLabel(gameResults.Score);
            endCardScreen.SetTimeLabel(gameResults.TimeTaken);
            endCardScreen.SetTotalMovesLabel(gameResults.TotalMoves);
            
            yield return sceneLoader.UnloadSceneAsync();
            yield return endCardScreen.Show();
        }
        
        private IEnumerator LoadMainMenu()
        {
            yield return loadingScreen.Show();
            yield return endCardScreen.Hide();
            yield return mainMenuScreen.Show();
            yield return loadingScreen.Hide();
        }

        private void OnPlayButtonClicked()
        {
            StartCoroutine(LoadSelectedGameMode(GameMode.Klondike));
        }
        
        private void OnPauseButtonClicked()
        {
            pauseScreen.UIDocument.rootVisualElement.visible = true;
            Time.timeScale = 0f;
        }
        
        private void OnResumeButtonClicked()
        {
            pauseScreen.UIDocument.rootVisualElement.visible = false;
            Time.timeScale = 1f;
        }

        private void OnMoveRegistered(IGameMode gameMode, IGameMove gameMove, int moveNumber)
        {
            dataTracker.TrackCardMove(gameMode, gameMove, moveNumber);
        }
        
        private void OnMoveUndone(IGameMode gameMode, IGameMove gameMove, int moveNumber)
        {
            dataTracker.TrackUndoMove(gameMode, gameMove, moveNumber);
        }

        private void OnGameFinished(IGameResults gameResults)
        {
            StartCoroutine(UnloadSelectedGameMode(gameResults));
            dataTracker.TrackGameEnd(gameResults);
        }
        
        private void OnEndCardContinueButtonClicked()
        {
            StartCoroutine(LoadMainMenu());
        }

        private void OnOptionsButtonClicked()
        {
            Debug.Log("Options");
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}