using System.Collections;
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
    }
}