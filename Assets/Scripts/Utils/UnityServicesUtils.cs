using System.Collections;
using Unity.Services.Core;

namespace Utils
{
    public static class UnityServicesUtils
    {
        public static IEnumerator InitializeUnityServices()
        {
            var task = UnityServices.InitializeAsync();
            yield return task;
        }
    }
}