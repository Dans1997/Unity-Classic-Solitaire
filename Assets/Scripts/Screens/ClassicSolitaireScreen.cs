using System;
using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace Screens
{
    public class ClassicSolitaireScreen : MonoBehaviour, IGameplayScreen
    {
        public event Action SettingsButtonClicked;
        public event Action ExitButtonClicked;
        public event Action PauseButtonClicked;
        public event Action UndoButtonClicked;
        
        [field: SerializeField] public UIDocument UIDocument { get; private set; }
        
        private VisualElement rootElement;
        private Button settingsButton;
        private Button exitButton;
        private Button pauseButton;
        private Button undoButton;
        private Label scoreLabel;
        private Label timeLabel;
        private Label movesLabel;
        private VisualElement cardContainer;
        private VisualElement[] cardColumns;
        
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
            
            if (settingsButton is null)
            {
                settingsButton = rootElement.Q<Button>("settings-button");
                settingsButton.text = "Settings";
                settingsButton.clicked += SettingsButtonClicked;
            }
            
            if (exitButton is null)
            {
                exitButton = rootElement.Q<Button>("exit-button");
                exitButton.text = "Exit";
                exitButton.clicked += ExitButtonClicked;
            }
            
            if (pauseButton is null)
            {
                pauseButton = rootElement.Q<Button>("pause-button");
                pauseButton.text = "Pause";
                pauseButton.clicked += PauseButtonClicked;
            }
            
            if (undoButton is null)
            {
                undoButton = rootElement.Q<Button>("undo-button");
                undoButton.text = "Undo";
                undoButton.clicked += UndoButtonClicked;
            }
            
            if (scoreLabel is null)
            {
                scoreLabel = rootElement.Q<Label>("score-label");
                scoreLabel.text = "0";
            }
            
            if (timeLabel is null)
            {
                timeLabel = rootElement.Q<Label>("time-label");
                timeLabel.text = "00:00";
            }
            
            if (movesLabel is null)
            {
                movesLabel = rootElement.Q<Label>("moves-label");
                movesLabel.text = "0";
            }

            if (cardContainer is null)
            {
                cardContainer = rootElement.Q<Label>("card-container");
            }

            if (cardColumns is null)
            {
                cardColumns = new []
                {
                    rootElement.Q("card-column-0"),
                    rootElement.Q("card-column-1"),
                    rootElement.Q("card-column-2"),
                    rootElement.Q("card-column-3"),
                    rootElement.Q("card-column-4"),
                    rootElement.Q("card-column-5"),
                    rootElement.Q("card-column-6")
                };
            }

            yield break;
        }
    }
}