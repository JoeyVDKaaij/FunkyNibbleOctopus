using Cysharp.Threading.Tasks;
using Game.Items;
using Game.Plates;
using UnityEngine;
using Zenject;

namespace Game.Tables
{
    public class TableController : MonoBehaviour, IItemAcceptor
    {
        [SerializeField]
        private Transform platePivot;

        [SerializeField]
        private TableCustomerSpawner customerSpawner;

        [SerializeField]
        private TableDisplay tableDisplay;

        [Inject]
        private PlatesController _platesController;

        [Inject]
        private TablesController _tablesController;

        private Plate _desiredPlate;
        public Plate DesiredPlate => _desiredPlate;

        private PlateController _currentPlate;

        private TableCustomer[] _customers;

        private void Start ()
        {
            _desiredPlate = _tablesController.RequestPlate();

            _customers = customerSpawner.SpawnCustomers();

            tableDisplay.ShowPlate(_desiredPlate.Type);
        }

        public void ClearCurrentPlate ()
        {
            Destroy(_currentPlate.gameObject);
            _currentPlate = null;
        }

        public bool IsItemAcceptable (object requester, IItem item)
        {
            if (_currentPlate != null)
                return false;

            if (requester is AIPathfinding && item is PlateController plate)
                return _desiredPlate.Type == plate.Plate.Type;

            return _desiredPlate != Plate.Invalid && item is PlateController && requester is MonoBehaviour;
        }

        public bool AcceptItem (object requester, IItem item)
        {
            if (item is not PlateController plate)
                return false;

            if (requester is AIPathfinding ai && _desiredPlate.Type == plate.Plate.Type)
                return AcceptPlate(ai.transform.position, plate);

            if ( _desiredPlate != Plate.Invalid && requester is MonoBehaviour behaviour)
                return AcceptPlate(behaviour.transform.position, plate);

            return false;
        }

        public bool AcceptPlate (Vector3 position, PlateController plate)
        {
            if (_currentPlate != null)
                return false;

            _currentPlate = plate;
            _currentPlate.SetParent(platePivot, Vector3.zero);

            ConsumePlate().Forget();

            return true;
        }

        private async UniTaskVoid ConsumePlate ()
        {
            tableDisplay.Hide();
            
            var desiredPlateType = _desiredPlate.Type;
            _desiredPlate = Plate.Invalid;

            await UniTask.Delay((int)(_tablesController.SecondsToConsume * 1000));
            
            if (_currentPlate.Plate.Type != desiredPlateType)
                for (int i = 0; i < _customers.Length; i++)
                    _customers[i].SignalAngry();
            else
                for (int i = 0; i < _customers.Length; i++)
                    _customers[i].SignalHappy();

            _platesController.ConsumePlate(_customers.Length, _currentPlate);

            await UniTask.Delay((int)(_tablesController.SecondsBetweenPlates * 1000));

            _currentPlate = null;

            for (int i = 0; i < _customers.Length; i++)
                _customers[i].SignalIdle();
            
            _desiredPlate = _tablesController.RequestPlate();

            tableDisplay.ShowPlate(_desiredPlate.Type);
        }
    }
}
