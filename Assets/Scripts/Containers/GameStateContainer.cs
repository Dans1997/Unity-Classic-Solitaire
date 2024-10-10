using System;
using System.Collections;
using Games.Klondike;
using Interfaces;
using Scenes;
using Screens;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;

namespace Containers
{
    public class GameStateContainer : MonoBehaviour
    {
        private IScreen loadingScreen;
        private IScreen mainMenuScreen;
        private SceneLoader sceneLoader;

        private IEnumerator Start()
        {
            yield return CreateLoadingScreen();
            yield return CreateMainMenuScreen();
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

        private void OnPlayButtonClicked()
        {
            StartCoroutine(LoadGameMode(GameMode.Klondike));
        }

        private void OnOptionsButtonClicked()
        {
            Debug.Log("Options");
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();
        }

        private IEnumerator LoadGameMode(GameMode gameMode)
        {
            yield return mainMenuScreen.Hide();
            yield return loadingScreen.Show();
            yield return sceneLoader.LoadScene(Constants.GameplaySceneKey);

            var gameModeController = gameMode switch
            {
                GameMode.Klondike => new KlondikeGameMode(Constants.ClassicSolitaireScreenPrefabKey),
                _ => throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null)
            };
            
            yield return gameModeController.DealCards();
            yield return loadingScreen.Hide();

            gameModeController.GameFinished += OnGameFinished;
            gameModeController.StartGame();
        }

        private void OnGameFinished()
        {
            Debug.Log("Game finished");
        }
    }
}