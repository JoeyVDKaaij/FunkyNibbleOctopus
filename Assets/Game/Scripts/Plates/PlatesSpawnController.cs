using System.Collections.Generic;
using Game.Camera;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Plates
{
    public class PlatesSpawnController : MonoBehaviour
    {
        [SerializeField]
        private CameraInstaller cameraInstaller;

        [SerializeField]
        private PlateController plateTemplate;

        [SerializeField]
        private PlateSpawnPoint spawnPointTemplate;

        [Header("Spawn Settings")]
        [SerializeField]
        private GameObject spawnParent;

        [SerializeField]
        private float spawnSpacing = 1f;

        [SerializeField]
        private Vector3 spawnOffset = Vector3.zero;

        [SerializeField]
        private Vector3 spawnPlane = Vector3.up;

        [SerializeField]
        private Vector3 spawnDirection = Vector3.forward;

        [Inject]
        private PlatesController _platesController;

        [Inject]
        private PlatesPickupController _platesPickupController;

        private PlateSpawnPoint[] _spawnPoints;

        private void Awake ()
        {
            _spawnPoints = new PlateSpawnPoint[_platesController.PlatesCount];
            for (int i = 0; i < _spawnPoints.Length; i++) {
                var spawnPoint = Instantiate(spawnPointTemplate, spawnParent.transform);
                spawnPoint.transform.localPosition = spawnOffset
                                                     + spawnDirection * spawnSpacing * i
                                                     - spawnDirection * spawnSpacing * (_platesController.PlatesCount - 1) * .5f;
                spawnPoint.transform.localRotation = Quaternion.LookRotation(spawnDirection, spawnPlane);
                _spawnPoints[i] = spawnPoint;
            }

            _platesPickupController.SetSpawnPoints(_spawnPoints);
        }

        private void OnEnable ()
        {
            _platesController.SpawnPlate += Spawn;
        }

        private void OnDisable ()
        {
            _platesController.SpawnPlate -= Spawn;
        }

        public PlateController Spawn (DiContainer container, string type)
        {
            var freeSpawnPoints = new List<PlateSpawnPoint>(_spawnPoints.Length);
            for (int i = 0; i < _spawnPoints.Length; i++)
                if (_spawnPoints[i].Plate == null)
                    freeSpawnPoints.Add(_spawnPoints[i]);
            if (freeSpawnPoints.Count == 0)
                return null;

            var spawnPoint = freeSpawnPoints[Random.Range(0, freeSpawnPoints.Count)];

            // container.Bind<UnityEngine.Camera>().FromInstance(cameraInstaller.Camera).NonLazy();
            var plate = container.InstantiatePrefabForComponent<PlateController>(plateTemplate, spawnPoint.transform);
            plate.transform.localPosition = Vector3.zero;
            plate.SetType(type);

            spawnPoint.Plate = plate;

            _platesController.RegisterPlate(plate);

            return plate;
        }
    }
}
