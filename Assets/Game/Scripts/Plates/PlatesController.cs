using System;
using UnityEngine;
using Zenject;

namespace Game.Plates
{
    public class PlatesController : MonoInstaller
    {
        public Action<object> OnPlateConsumed;

        public Action OnAllPlatesConsumed;

        [SerializeField, Min(0)]
        private int platesCount;

        public object[] CurrentPlates { get; private set; }

        public override void InstallBindings ()
        {
            Container.Bind<PlatesController>().FromInstance(this).AsSingle().NonLazy();
        }

        public void RenewPlates ()
        {
            CurrentPlates = new object[platesCount];
            for (int i = 0; i < CurrentPlates.Length; i++)
                CurrentPlates[i] = new object();
        }

        public bool IsPlateConsumed (object plate)
        {
            if (plate == null || CurrentPlates == null || CurrentPlates.Length == 0)
                return false;

            return Array.IndexOf(CurrentPlates, plate) >= 0;
        }

        public bool ConsumePlate (object plate)
        {
            if (plate == null || CurrentPlates == null || CurrentPlates.Length == 0)
                return false;

            var index = Array.IndexOf(CurrentPlates, plate);
            if (index < 0)
                return false;

            CurrentPlates[index] = null;

            OnPlateConsumed?.Invoke(plate);

            bool allPlatesConsumed = true;
            for (int i = 0; i < CurrentPlates.Length; i++) {
                if (CurrentPlates[i] != null) {
                    allPlatesConsumed = false;
                    break;
                }
            }
            if (allPlatesConsumed)
                OnAllPlatesConsumed?.Invoke();

            return true;
        }
    }
}
