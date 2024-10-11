using System;
using System.Collections;
using Interfaces;
using UnityEngine.AddressableAssets;

namespace Utils
{
    public static class AddressablesUtils
    {
        public static IEnumerator InitializeAddressables()
        {
            var handle = Addressables.InitializeAsync(true);
            yield return handle;
        }
        
        public static IEnumerator CreateScreen<T>(string prefabKey, Action<T> onScreenCreated) where T : IScreen
        {
            var handle = Addressables.InstantiateAsync(prefabKey);
            yield return handle;
            
            var screen = handle.Result.GetComponent<T>();
            onScreenCreated?.Invoke(screen);
            
            yield return screen.Show();
        }
    }
}