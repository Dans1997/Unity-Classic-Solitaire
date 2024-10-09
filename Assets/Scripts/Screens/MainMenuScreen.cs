using System;
using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace Screens
{
    public class MainMenuScreen : MonoBehaviour, IScreen
    {
        public event Action PlayButtonClicked;
        public event Action OptionsButtonClicked;
        public event Action ExitButtonClicked;

        [field: SerializeField] public UIDocument UIDocument { get; private set; }
        
        private VisualElement rootElement;
        private Button playButton;
        private Button optionsButton;
        private Button exitButton;

        public IEnumerator Show()
        {
            yield return Setup();
            rootElement.visible = true;
        }

        public IEnumerator Hide()
        {
            rootElement.visible = false;
            yield break;
        }
        
        private IEnumerator Setup()
        {
            rootElement ??= UIDocument.rootVisualElement;
            
            if (playButton is null)
            {
                playButton = rootElement.Q<Button>("play-button");
                playButton.clicked += PlayButtonClicked;
            }

            if (optionsButton is null)
            {
                optionsButton = rootElement.Q<Button>("options-button");
                optionsButton.clicked += OptionsButtonClicked;
            }
            
            if (exitButton is null)
            {
                exitButton = rootElement.Q<Button>("exit-button");
                exitButton.clicked += ExitButtonClicked;
            }

            yield break;
        }
    }
}