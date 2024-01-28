using Game.Waves;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class WaveDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        [Inject]
        private WavesController _wavesController;

        private void OnEnable ()
        {
            _wavesController.OnWaveCompleted += OnWaveCompleted_SetText;
        }

        private void OnDisable ()
        {
            _wavesController.OnWaveCompleted -= OnWaveCompleted_SetText;
        }

        private void OnWaveCompleted_SetText (int waveNumber)
        {
            text.text = waveNumber.ToString("D2") + "/" + _wavesController.MaximumWaveCount.ToString("D2");
        }
    }
}
