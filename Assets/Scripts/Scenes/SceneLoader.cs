using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class SceneLoader
    {
        private readonly Scene gameStateScene;
        private AsyncOperationHandle<SceneInstance> handle;

        public SceneLoader(Scene gameStateScene)
        {
            this.gameStateScene = gameStateScene;
        }
        
        public IEnumerator LoadSceneAsync(string sceneKey)
        {
            handle = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Additive);
            yield return handle;
            SceneManager.SetActiveScene(handle.Result.Scene);
        }

        public IEnumerator UnloadSceneAsync()
        {
            yield return Addressables.UnloadSceneAsync(handle.Result);
            SceneManager.SetActiveScene(gameStateScene);
        }
    }
}