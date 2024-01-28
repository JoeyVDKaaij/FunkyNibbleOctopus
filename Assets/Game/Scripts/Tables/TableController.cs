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
        private TableDisplay tableDisplay;

        [Inject]
        private PlatesController _platesController;

        [Inject]
        private TablesController _tablesController;

        private Plate _desiredPlate;

        private PlateController _currentPlate;

        private void Start ()
        {
            _desiredPlate = _tablesController.RequestPlate();

            tableDisplay.ShowPlate(_desiredPlate.Type);
        }

        public void ClearCurrentPlate ()
        {
            Destroy(_currentPlate.gameObject);
            _currentPlate = null;
        }

        public bool IsItemAcceptable (object requester, IItem item)
        {
            return _desiredPlate != Plate.Invalid && _currentPlate == null && item is PlateController && requester is MonoBehaviour;
        }

        public bool AcceptItem (object requester, IItem item)
        {
            if ( _desiredPlate != Plate.Invalid && item is PlateController plate && requester is MonoBehaviour behaviour)
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
            _desiredPlate = Plate.Invalid;

            await UniTask.Delay((int)(_tablesController.SecondsToConsume * 1000));

            _platesController.ConsumePlate(_currentPlate);
            _currentPlate = null;

            await UniTask.Delay((int)(_tablesController.SecondsBetweenPlates * 1000));

            _desiredPlate = _tablesController.RequestPlate();

            tableDisplay.ShowPlate(_desiredPlate.Type);
        }
    }
}
