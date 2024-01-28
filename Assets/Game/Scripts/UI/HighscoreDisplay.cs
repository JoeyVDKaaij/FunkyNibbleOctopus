using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class HighscoreDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        [Inject]
        private GameManager _gameManager;

        private float _score;

        private void Start ()
        {
            text.text = Mathf.RoundToInt(_gameManager.Highscore).ToString("D4");
        }
    }
}
