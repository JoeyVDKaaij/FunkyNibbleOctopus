using Game.Waves;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class WaveDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        [SerializeField]
        private Image timer;

        [Inject]
        private WavesController _wavesController;

        private void Start ()
        {
            text.text = _wavesController.WaveNumber.ToString("D2") + "/" + _wavesController.MaximumWaveCount.ToString("D2");
        }

        private void OnEnable ()
        {
            _wavesController.OnWaveCompleted += OnWaveCompleted_SetText;
        }

        private void OnDisable ()
        {
            _wavesController.OnWaveCompleted -= OnWaveCompleted_SetText;
        }

        private void Update ()
        {
            if (_wavesController.WaveNumber > _wavesController.MaximumWaveCount) {
                timer.fillAmount = 0f;
                return;
            }

            timer.fillAmount = _wavesController.TimeToNextWave / _wavesController.MaximumWaveDuration;
        }

        private void OnWaveCompleted_SetText (int waveNumber)
        {
            if (waveNumber > _wavesController.MaximumWaveCount)
                return;

            text.text = waveNumber.ToString("D2") + "/" + _wavesController.MaximumWaveCount.ToString("D2");
        }
    }
}
