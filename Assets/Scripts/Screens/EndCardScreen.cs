using System;
using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace Screens
{
    public class EndCardScreen : MonoBehaviour, IGameResultsScreen
    {
        public event Action ContinueButtonClicked;
        
        [field: SerializeField] public UIDocument UIDocument { get; private set; }
        
        private VisualElement rootElement;
        private Label gameResultLabel;
        private Label scoreLabel;
        private Label timeLabel;
        private Label totalMoves;
        private Button continueButton;
        
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

        public void SetGameResultLabel(string text) => gameResultLabel.text = text;
        public void SetScoreLabel(int gameResultsScore) => scoreLabel.text = gameResultsScore.ToString();
        public void SetTimeLabel(float timeTaken) => timeLabel.text = TimeUtils.PrintTime(timeTaken);
        public void SetTotalMovesLabel(int moves) => totalMoves.text = moves.ToString();
        
        private IEnumerator Setup()
        {
            rootElement ??= UIDocument.rootVisualElement;
            gameResultLabel ??= rootElement.Q<Label>("result-label");
            scoreLabel ??= rootElement.Q<Label>("score-label");
            timeLabel ??= rootElement.Q<Label>("time-label");
            totalMoves ??= rootElement.Q<Label>("moves-label");
            
            if (continueButton is null)
            {
                continueButton = rootElement.Q<Button>("continue-button");
                continueButton.clicked += ContinueButtonClicked;
            }
            
            yield break;
        }
    }
}