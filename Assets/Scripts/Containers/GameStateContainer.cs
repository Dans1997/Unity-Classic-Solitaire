using System;
using System.Collections;
using Games.Klondike;
using Interfaces;
using Scenes;
using Screens;
using Timers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;

namespace Containers
{
    public class GameStateContainer : MonoBehaviour
    {
        private IScreen loadingScreen;
        private IScreen mainMenuScreen;
        private IScreen pauseScreen;
        private IGameResultsScreen endCardScreen;
        private SceneLoader sceneLoader;

        private IEnumerator Start()
        {
            yield return AddressablesUtils.InitializeAddressables();
            yield return CreateLoadingScreen();
            yield return CreateMainMenuScreen();
            yield return CreatePauseScreen();
            yield return CreateEndCardScreen();
            yield return loadingScreen.Hide();
            
            sceneLoader = new SceneLoader(loadingScreen);
        }
        
        private IEnumerator CreateLoadingScreen()
        {
            var handle = Addressables.InstantiateAsync(Constants.LoadingScreenPrefabKey);
            yield return handle;
            loadingScreen = handle.Result.GetComponent<IScreen>();
        }
        
        private IEnumerator CreateMainMenuScreen()
        {
            var handle = Addressables.InstantiateAsync(Constants.MainMenuScreenPrefabKey);
            yield return handle;
            
            var mainMenu = handle.Result.GetComponent<MainMenuScreen>();
            mainMenu.PlayButtonClicked += OnPlayButtonClicked;
            mainMenu.OptionsButtonClicked += OnOptionsButtonClicked;
            mainMenu.ExitButtonClicked += OnExitButtonClicked;
            mainMenuScreen = mainMenu;
            
            yield return mainMenuScreen.Show();
        }
        
        private IEnumerator CreatePauseScreen()
        {
            var handle = Addressables.InstantiateAsync(Constants.PauseScreenPrefabKey);
            yield return handle;
            var pause = handle.Result.GetComponent<PauseScreen>();
            pause.ResumeButtonClicked += OnResumeButtonClicked;
            
            pauseScreen = pause;
            yield return pauseScreen.Show();
            yield return pauseScreen.Hide();
        }

        private IEnumerator CreateEndCardScreen()
        {
            var handle = Addressables.InstantiateAsync(Constants.EndCardScreenPrefabKey);
            yield return handle;
            var endCard = handle.Result.GetComponent<EndCardScreen>();
            endCard.ContinueButtonClicked += OnEndCardContinueButtonClicked;
            
            endCardScreen = endCard;
            yield return endCardScreen.Show();
            yield return endCardScreen.Hide();
        }

        private IEnumerator LoadGameMode(GameMode gameMode)
        {
            yield return mainMenuScreen.Hide();
            yield return loadingScreen.Show();
            yield return sceneLoader.LoadScene(Constants.GameplaySceneKey);
            var gameModeController = gameMode switch
            {
                GameMode.Klondike => new KlondikeGameMode(new CoroutineTimer(this), 
                    Constants.ClassicSolitaireScreenPrefabKey),
                _ => throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null)
            };
            
            yield return gameModeController.DealCards();
            yield return loadingScreen.Hide();

            gameModeController.PauseButtonClicked += OnPauseButtonClicked;
            gameModeController.GameFinished += OnGameFinished;
            gameModeController.StartGame();
        }

        private void OnPlayButtonClicked()
        {
            StartCoroutine(LoadGameMode(GameMode.Klondike));
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

        private void OnGameFinished(IGameResults gameResults)
        {
            Debug.Log($"Game finished with results: {gameResults}");
            
            endCardScreen.SetGameResultLabel($"You {gameResults.GameOutcome}!");
            endCardScreen.SetScoreLabel(gameResults.Score);
            endCardScreen.SetTimeLabel(gameResults.TimeTaken);
            endCardScreen.SetTotalMovesLabel(gameResults.TotalMoves);
            
            StartCoroutine(endCardScreen.Show());
        }
        
        private void OnEndCardContinueButtonClicked()
        {
            StartCoroutine(GoBackToMainMenu());
            return;
            
            IEnumerator GoBackToMainMenu()
            {
                yield return loadingScreen.Show();
                yield return endCardScreen.Hide();
                yield return mainMenuScreen.Show();
                yield return loadingScreen.Hide();
            }
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