using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace Screens
{
    public class LoadingScreen : MonoBehaviour, IScreen
    {
        [field: SerializeField] public UIDocument UIDocument { get; private set; }
        
        public IEnumerator Show()
        {
            UIDocument.rootVisualElement.visible = true;
            yield break;
        }

        public IEnumerator Hide()
        {
            UIDocument.rootVisualElement.visible = false;
            yield break;
        }
    }
}