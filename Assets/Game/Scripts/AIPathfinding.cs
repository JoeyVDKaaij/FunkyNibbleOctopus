using System.Collections.Generic;
using Game.Items;
using Game.Plates;
using Game.Tables;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIPathfinding : MonoBehaviour
    {
        [SerializeField]
        private float timeout = 4f;

        private PlateController _desiredPlate;
        private TableController _reservedTable;

        private IItem _currentItem;

        private NavMeshAgent agent = null;

        [Inject]
        private PlatesController _platesController;

        [Inject]
        private TablesController _tablesController;

        private float _allowedInteractionMoment;

        private float _nextTimeTimeout;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable ()
        {
            _platesController.OnPlateConsumed += OnPlateConsumed_GetNewFocusDish;
        }

        private void OnDisable ()
        {
            _platesController.OnPlateConsumed -= OnPlateConsumed_GetNewFocusDish;
        }

        private void Update()
        {
            if (_desiredPlate == null)
            {
                if (_platesController.CurrentPlates.Count == 0) {
                    agent.SetDestination(transform.position);

                    return;
                }

                List<PlateController> plates;
                if (_currentItem == null)
                    plates = _platesController.CurrentPlates;
                else
                    plates = new List<PlateController> { _currentItem as PlateController };
                _reservedTable = _tablesController.ReserveTable(plates);
                if (_reservedTable == null) {
                    agent.SetDestination(transform.position);

                    return;
                }

                foreach (var plate in _platesController.CurrentPlates)
                    if (plate.Plate.Type == _reservedTable.DesiredPlate.Type)
                    {
                        _desiredPlate = plate;
                        break;
                    }
            }

            Vector3 targetPosition = _currentItem == null
                ? _desiredPlate.transform.position
                : _reservedTable.transform.position;
            if (agent.destination == targetPosition && Time.time > _nextTimeTimeout) {
                // TODO: consider if it makes sense to ever get rid of the attached food
                if (_currentItem != null) {
                    _tablesController.FreeTable(_reservedTable);
                    _reservedTable = null;
                    _desiredPlate = null;
                }
            } else if (agent.destination != targetPosition)
                agent.SetDestination(targetPosition);

            _nextTimeTimeout = Time.time + timeout;
        }

        private void OnTriggerStay (Collider other)
        {
            if (Time.time < _allowedInteractionMoment)
                return;

            if (_currentItem == null && other.TryGetComponent<IItemProvider>(out var itemProvider)) {
                var item = itemProvider.GetItem(this);
                if (item != null)
                    HoldItem(item);
            } else if (_currentItem != null && other.TryGetComponent<IItemAcceptor>(out var itemAcceptor)) {
                if (itemAcceptor.IsItemAcceptable(this, _currentItem))
                {
                    _ = itemAcceptor.AcceptItem(this, _currentItem);
                    _currentItem = null;
                    _tablesController.FreeTable(_reservedTable);
                    _reservedTable = null;
                    _desiredPlate = null;
                }
            }

            _allowedInteractionMoment = Time.time + 2f /* seconds in the future */;
        }

        private void HoldItem (IItem item)
        {
            _currentItem = item;
            _currentItem.SetParent(transform, new Vector3(0, 1.2f, 0));
        }

        private void OnPlateConsumed_GetNewFocusDish(int customersCount, PlateController plate)
        {
            if (plate == _desiredPlate) {
                _tablesController.FreeTable(_reservedTable);
                _reservedTable = null;
                _desiredPlate = null;
            }
        }

        public IItem CurrentPlate
        {
            get { return _currentItem; }
        }

        public void RemovePlate()
        {
            _currentItem = null;
            _tablesController.FreeTable(_reservedTable);
            _reservedTable = null;
            _desiredPlate = null;
        }
    }
}
