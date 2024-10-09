using System.Collections;
using Interfaces;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class SceneLoader
    {
        private readonly IScreen loadingScreen;
        private AsyncOperationHandle<SceneInstance> handle;

        public SceneLoader(IScreen loadingScreen)
        {
            this.loadingScreen = loadingScreen;
        }
        
        public IEnumerator LoadScene(string sceneKey)
        {
            yield return loadingScreen.Show();
            handle = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Additive);
            yield return handle;
            yield return loadingScreen.Hide();
        }

        public IEnumerator UnloadScene()
        {
            yield return loadingScreen.Show();
            yield return Addressables.UnloadSceneAsync(handle.Result);
            yield return loadingScreen.Hide();
        }
    }
}