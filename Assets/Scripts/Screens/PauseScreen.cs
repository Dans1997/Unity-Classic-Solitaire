using System;
using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace Screens
{
    public class PauseScreen : MonoBehaviour, IScreen
    {
        public event Action ResumeButtonClicked;
        
        [field: SerializeField] public UIDocument UIDocument { get; private set; }
        
        private VisualElement rootElement;
        private Button resumeButton;
        
        public IEnumerator Show()
        {
            yield return Setup();
            UIDocument.rootVisualElement.visible = true;
        }

        public IEnumerator Hide()
        {
            UIDocument.rootVisualElement.visible = false;
            yield break;
        }
        
        private IEnumerator Setup()
        {
            rootElement ??= UIDocument.rootVisualElement;
            
            if (resumeButton is null)
            {
                resumeButton = rootElement.Q<Button>("resume-button");
                resumeButton.clicked += ResumeButtonClicked;
            }
            
            yield break;
        }
    }
}