using System;
using System.Collections.Generic;
using Game.Plates;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

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

        private bool[] _isTableReserved;

        public override void InstallBindings ()
        {
            Container.Bind<TablesController>().FromInstance(this).AsSingle().NonLazy();
        }

        private void Awake ()
        {
            _isTableReserved = new bool[tables.Length];
        }

        public Plate RequestPlate ()
        {
            string type = _platesRepository.PlateContainers[Random.Range(0, _platesRepository.PlateContainers.Length)].Type;

            return Plate.Create(type);
        }

        public TableController ReserveTable (List<PlateController> availablePlates)
        {
            for (int i = 0; i < tables.Length; i++)
                if (!_isTableReserved[i] && tables[i].DesiredPlate != Plate.Invalid)
                    for (int j = 0; j < availablePlates.Count; j++)
                        if (tables[i].DesiredPlate.Type == availablePlates[j].Plate.Type) {
                            _isTableReserved[i] = true;
                            return tables[i];
                        }

            return null;
        }

        public void FreeTable (TableController table)
        {
            for (int i = 0; i < tables.Length; i++)
                if (tables[i] == table) {
                    _isTableReserved[i] = false;
                    break;
                }
        }
    }
}
