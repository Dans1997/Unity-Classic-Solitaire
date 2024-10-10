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
        
        public VisualElement FindColumn(string className) => rootElement.Q<VisualElement>(className);
        public void SetMoveCount(int movesCount) => movesLabel.text = movesCount.ToString();
        public void SetTime(float time)
        {
            var minutes = Mathf.FloorToInt(time / 60);
            var seconds = Mathf.FloorToInt(time % 60);
            timeLabel.text = $"{minutes:00}:{seconds:00}";
        }
        public void SetScore(int score) => scoreLabel.text = score.ToString();

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

            yield break;
        }
    }
}