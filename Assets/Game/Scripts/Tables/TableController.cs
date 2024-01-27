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

        [SerializeField, Range(0, 10)]
        private float secondsToConsume = 1f;

        [Inject]
        private PlatesController _platesController;

        private PlateController _currentPlate;

        public void ClearCurrentPlate ()
        {
            Destroy(_currentPlate.gameObject);
            _currentPlate = null;
        }

        public bool IsItemAcceptable (object requester, IItem item)
        {
            return _currentPlate == null && item is PlateController && requester is MonoBehaviour;
        }

        public bool AcceptItem (object requester, IItem item)
        {
            if (item is PlateController plate && requester is MonoBehaviour behaviour)
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
            await UniTask.Delay((int)(secondsToConsume * 1000));

            _platesController.ConsumePlate(_currentPlate);

            _currentPlate = null;
        }
    }
}
