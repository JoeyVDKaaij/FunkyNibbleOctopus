using Game.Plates;
using UnityEngine;
using Zenject;

namespace Game.Tables
{
    public class TablesController : MonoInstaller
    {
        [SerializeField]
        private TableController[] tables;

        [SerializeField, Range(0, 10)]
        private float secondsToConsume = 1f;
        public float SecondsToConsume => secondsToConsume;

        [SerializeField, Range(0, 10)]
        private float secondsBetweenPlates = 5f;
        public float SecondsBetweenPlates => secondsBetweenPlates;

        [Inject]
        private PlatesRepository _platesRepository;

        public override void InstallBindings ()
        {
            Container.Bind<TablesController>().FromInstance(this).AsSingle().NonLazy();
        }

        public Plate RequestPlate ()
        {
            string type = _platesRepository.PlateContainers[Random.Range(0, _platesRepository.PlateContainers.Length)].Type;

            return Plate.Create(type);
        }
    }
}
