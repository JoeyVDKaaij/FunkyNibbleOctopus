using UnityEngine;
using Zenject;

namespace Game
{
    public class GameStateController : MonoInstaller
    {
        [Inject]
        private GameManager _gameManager;

        [Inject]
        private Waves.WavesController _wavesController;

        [Inject]
        private Plates.PlatesController _platesController;

        [SerializeField]
        private float score;

        public float Score
        {
            get => score;
            private set => score = value;
        }

        public override void InstallBindings ()
        {
            Container.Bind<GameStateController>().FromInstance(this).AsSingle().NonLazy();
        }

        private void Start ()
        {
            _platesController.RenewPlates();
        }

        private void OnEnable ()
        {
            _wavesController.OnAllWavesCompleted += OnAllWavesCompleted_FinishGame;
            _platesController.OnPlateConsumed += OnPlateConsumed_AddScore;
            _platesController.OnAllPlatesConsumed += OnAllPlatesConsumed_NextWave;
        }

        private void Update ()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.C)) {
                object plate = null;
                for (int i = 0; i < _platesController.CurrentPlates.Length; i++) {
                    if (_platesController.CurrentPlates[i] != null) {
                        plate = _platesController.CurrentPlates[i];
                        break;
                    }
                }
                if (plate != null)
                    _ = _platesController.ConsumePlate(plate);
            }
            if (Input.GetKeyDown(KeyCode.N))
                _wavesController.NextWave();
#endif
        }

        private void OnAllWavesCompleted_FinishGame ()
        {
            // TODO: add proper win/lose UI
            _gameManager.FinishGame(score);
        }

        private void OnPlateConsumed_AddScore (object plate)
        {
            // TODO: add proper score calculation
            score += 100;
        }

        private void OnAllPlatesConsumed_NextWave ()
        {
            _platesController.RenewPlates();
            _wavesController.NextWave();
        }
    }
}
