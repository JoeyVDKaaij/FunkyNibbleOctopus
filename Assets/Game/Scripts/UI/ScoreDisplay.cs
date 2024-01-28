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
        
        private float _score;

        private void Update ()
        {
            _score = Mathf.Lerp(_score, _gameStateController.Score, Time.deltaTime * 10f);
            text.text = Mathf.RoundToInt(_score).ToString("D4");
        }
    }
}
