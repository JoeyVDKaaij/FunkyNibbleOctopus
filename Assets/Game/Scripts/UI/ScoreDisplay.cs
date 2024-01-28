using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class ScoreDisplay : MonoBehaviour 
    {
        [SerializeField]
        private TMP_Text text;

        [Inject]
        private GameStateController _gameStateController;

        private void OnEnable ()
        {
            _gameStateController.OnScoreChanged += OnScoreChanged_SetText;
        }

        private void OnDisable ()
        {
            _gameStateController.OnScoreChanged -= OnScoreChanged_SetText;
        }

        private void OnScoreChanged_SetText (float previousScore, float currentScore)
        {
            text.text = ((int)currentScore).ToString("D4");
        }
    }
}
