using System;
using UnityEngine;
using Zenject;

namespace Game.Waves
{
    public class WavesController : MonoInstaller
    {
        public Action OnAllWavesCompleted;

        [SerializeField]
        private int maximumWaveCount;

        [SerializeField]
        private int currentWaveNumber;
        public int WaveNumber => currentWaveNumber;

        [Inject]
        private GameManager _gameManager;

        public override void InstallBindings ()
        {
            Container.Bind<WavesController>().FromInstance(this).AsSingle().NonLazy();
        }

        public void NextWave ()
        {
            currentWaveNumber++;
            if (currentWaveNumber > maximumWaveCount) {
                OnAllWavesCompleted?.Invoke();
#if UNITY_EDITOR
                // TODO: move to a game state manager
                _gameManager.FinishGame(0);
#endif
            }
        }

        private void Update ()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.N))
                NextWave();
#endif
        }
    }
}
