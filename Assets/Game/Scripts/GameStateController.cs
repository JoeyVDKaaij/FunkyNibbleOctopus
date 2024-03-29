﻿using System;
using Game.Plates;
using Game.Tables;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameStateController : MonoInstaller
    {
        public event Action<float, float> OnScoreChanged;

        [Inject]
        private GameManager _gameManager;

        [Inject]
        private Waves.WavesController _wavesController;

        [Inject]
        private PlatesController _platesController;

        [SerializeField]
        private float score;
        public float Score
        {
            get => score;
            private set => score = value;
        }

        [SerializeField, Min(0)]
        private float scoreLossRatePerSecond;

        [SerializeField, Min(0)]
        private float scoreLossPerCustomerPerCorrectDelivery;

        [SerializeField, Min(0)]
        private float scoreGainPerCustomerPerIncorrectDelivery;

        public override void InstallBindings ()
        {
            Container.Bind<GameStateController>().FromInstance(this).AsSingle().NonLazy();
        }

        public override void Start ()
        {
            base.Start();

            _platesController.RenewPlates();
        }

        private void OnEnable ()
        {
            _wavesController.OnWaveCompleted += OnWaveCompleted_RenewPlates;
            _wavesController.OnAllWavesCompleted += OnAllWavesCompleted_FinishGame;
            _platesController.OnPlateConsumed += OnPlateConsumed_AddScore;
            _platesController.OnAllPlatesConsumed += OnAllPlatesConsumed_NextWave;
        }
        
        private void OnDisable ()
        {
            _wavesController.OnWaveCompleted -= OnWaveCompleted_RenewPlates;
            _wavesController.OnAllWavesCompleted -= OnAllWavesCompleted_FinishGame;
            _platesController.OnPlateConsumed -= OnPlateConsumed_AddScore;
            _platesController.OnAllPlatesConsumed -= OnAllPlatesConsumed_NextWave;
        }

        private void Update ()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.C)) {
                PlateController plate = null;
                for (int i = 0; i < _platesController.CurrentPlates.Count; i++) {
                    if (_platesController.CurrentPlates[i] != null) {
                        plate = _platesController.CurrentPlates[i];
                        break;
                    }
                }
                if (plate != null)
                    _ = _platesController.ConsumePlate(0, plate, Plate.Invalid);
            }
            if (Input.GetKeyDown(KeyCode.N))
                _wavesController.NextWave();
#endif
            score -= scoreLossRatePerSecond * Time.deltaTime;
        }

        private void OnWaveCompleted_RenewPlates (int waveNumber)
        {
            _platesController.RenewPlates();
        }

        private void OnAllWavesCompleted_FinishGame ()
        {
            // TODO: add proper win/lose UI
            _gameManager.FinishGame(score);
        }

        private void OnPlateConsumed_AddScore (int customersCount, PlateController plate, Plate expectedPlate)
        {
            if (plate == null)
                return;

            float previousScore = score;
            if (expectedPlate.Type != plate.Plate.Type)
                score += scoreGainPerCustomerPerIncorrectDelivery * customersCount;
            else
                score -= scoreLossPerCustomerPerCorrectDelivery * customersCount;

            OnScoreChanged?.Invoke(previousScore, score);
        }

        private void OnAllPlatesConsumed_NextWave ()
        {
            _platesController.RenewPlates();
            _wavesController.NextWave();
        }
    }
}
