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

        [SerializeField, Min(1)]
        private int currentWaveNumber;
        public int WaveNumber => currentWaveNumber;

        public override void InstallBindings ()
        {
            Container.Bind<WavesController>().FromInstance(this).AsSingle().NonLazy();
        }

        public void NextWave ()
        {
            currentWaveNumber++;
            if (currentWaveNumber > maximumWaveCount)
                OnAllWavesCompleted?.Invoke();
        }
    }
}
