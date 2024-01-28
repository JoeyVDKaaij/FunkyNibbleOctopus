using System;
using UnityEngine;
using Zenject;

namespace Game.Waves
{
    public class WavesController : MonoInstaller
    {
        public event Action<int> OnWaveCompleted;

        public event Action OnAllWavesCompleted;

        [SerializeField, Range(1f, 60f)]
        private float maximumWaveDuration;
        public float MaximumWaveDuration => maximumWaveDuration;

        [SerializeField]
        private int maximumWaveCount;
        public int MaximumWaveCount => maximumWaveCount;

        [SerializeField, Min(1)]
        private int currentWaveNumber;

        public int WaveNumber => currentWaveNumber;

        private float _nextWaveThreshold;
        public float TimeToNextWave => Mathf.Max(0f, _nextWaveThreshold - Time.time);

        public override void InstallBindings ()
        {
            Container.Bind<WavesController>().FromInstance(this).AsSingle().NonLazy();
        }

        private void Update ()
        {
            if (Time.time > _nextWaveThreshold)
                NextWave();
        }

        public void NextWave ()
        {
            currentWaveNumber++;
            OnWaveCompleted?.Invoke(currentWaveNumber);
            if (currentWaveNumber > maximumWaveCount)
                OnAllWavesCompleted?.Invoke();

            _nextWaveThreshold = Time.time + maximumWaveDuration;
        }
    }
}
