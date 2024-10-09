using System.Collections;
using Containers;
using Interfaces;
using Scenes;
using Screens;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;

namespace Handlers
{
    public class GameStateHandler : MonoBehaviour
    {
        private IScreen loadingScreen;
        private IScreen mainMenuScreen;
        private SceneLoader sceneLoader;
        private GameStateContainer gameStateContainer;

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
            Debug.Log("Play");
            StartCoroutine(LoadGameplayScene());
        }

        private void OnOptionsButtonClicked()
        {
            Debug.Log("Options");
        }

        private void OnExitButtonClicked()
        {
            Debug.Log("Exit");
            Application.Quit();
        }

        private IEnumerator LoadGameplayScene()
        {
            yield return mainMenuScreen.Hide();
            yield return loadingScreen.Show();
            yield return sceneLoader.LoadScene(Constants.GameplaySceneKey);
            yield return loadingScreen.Hide();
            
            // TODO: gameStateContainer.StartGame();
        }
        
        public void SelectGameMode(IGameModeFactory gameModeFactory)
        {
            gameStateContainer = new GameStateContainer(gameModeFactory.CreateGameMode());
            StartCoroutine(LoadGameplayScene());
        }

        public void UndoMove()
        {
            gameStateContainer.UndoMove();
        }
    }
}