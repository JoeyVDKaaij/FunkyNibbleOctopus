using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Plates
{
    public class PlatesController : MonoInstaller
    {
        public event Action<int, PlateController> OnPlateConsumed;

        public event Action OnAllPlatesConsumed;

        public delegate List<PlateController> PlateSpawner (DiContainer container, int count);

        public event PlateSpawner SpawnPlate;

        [SerializeField, Min(0)]
        private int platesCount;
        public int PlatesCount => platesCount;

        public List<PlateController> CurrentPlates { get; private set; }

        [Inject]
        private PlatesRepository _platesRepository;

        public override void InstallBindings ()
        {
            Container.Bind<PlatesController>().FromInstance(this).AsSingle().NonLazy();
        }

        public void RenewPlates ()
        {
            if (CurrentPlates == null)
                CurrentPlates = new List<PlateController>(platesCount);

            CurrentPlates.Clear();
            CurrentPlates = SpawnPlate?.Invoke(Container, platesCount);
        }

        public bool IsPlateConsumed (PlateController plate)
        {
            if (plate == null || CurrentPlates == null || CurrentPlates.Count == 0)
                return false;

            return CurrentPlates.Contains(plate);
        }

        public void RegisterPlate (PlateController plate)
        {
            if (plate == null)
                return;

            CurrentPlates ??= new List<PlateController>(platesCount);

            CurrentPlates.Add(plate);
        }

        public bool ConsumePlate (int customerCount, PlateController plate)
        {
            if (plate == null)
                return false;

            var index = CurrentPlates.IndexOf(plate);
            if (index >= 0)
                CurrentPlates.RemoveAt(index);

            OnPlateConsumed?.Invoke(customerCount, plate);
            if (CurrentPlates.Count == 0)
                OnAllPlatesConsumed?.Invoke();

            Destroy(plate.gameObject);

            return true;
        }
    }
}
