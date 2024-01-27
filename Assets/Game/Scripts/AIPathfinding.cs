using UnityEngine;
using UnityEngine.AI;
using Game.Items;
using Game.Plates;
using Zenject;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class AIPathfinding : MonoBehaviour
{
    [Header("AI Pathfinding Settings")] [SerializeField, Tooltip("The tables that this GameObject has to pay attention to.")]
    private Transform[] tables = null;
    [SerializeField, Tooltip("The dishes that this GameObject has to pay attention to.")]
    private PlateController[] dishes = null;

    private int focusDishId = 0;
    private IItem _currentItem;
    private bool _isItemInteractionRequested = true;
    private Transform _childObject;
    
    private NavMeshAgent agent = null;

    [Inject]
    private PlatesController _platesController;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        focusDishId = Random.Range(0, dishes.Length);
    }

    private void Start()
    {
        dishes = _platesController.CurrentPlates.ToArray();
    }

    private void OnEnable ()
    {
        _platesController.OnPlateConsumed += OnPlateConsumed_GetNewFocusDish;
    }

    private void OnDisable ()
    {
        _platesController.OnPlateConsumed -= OnPlateConsumed_GetNewFocusDish;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_currentItem == null)
        {
            agent.SetDestination(dishes[focusDishId].transform.position);
        }
        else
        {
            agent.SetDestination(tables[0].transform.position);
        }
    }

    
    private void OnTriggerStay (Collider other)
        {
            if (!_isItemInteractionRequested) 
                return;
            
            if (_currentItem == null && other.TryGetComponent<IItemProvider>(out var itemProvider)) {
                var item = itemProvider.GetItem(transform.position);
                if (item != null)
                    HoldItem(item);
            } else if (_currentItem != null && other.TryGetComponent<IItemAcceptor>(out var itemAcceptor)) {
                if (itemAcceptor.IsItemAcceptable(transform.position, _currentItem))
                {
                    _ = itemAcceptor.AcceptItem(transform.position, _currentItem);
                    _currentItem = null;
                }
            }

            _isItemInteractionRequested = false;
        }

        private void HoldItem (IItem item)
        {
            _currentItem = item;
            _currentItem.SetParent(transform, new Vector3(0, 1.2f, 0));
        }

    private void OnPlateConsumed_GetNewFocusDish(PlateController plate)
    {
        if (_platesController.CurrentPlates.IndexOf(plate) == focusDishId) {
            dishes = _platesController.CurrentPlates.ToArray();
            focusDishId = Random.Range(0, _platesController.CurrentPlates.Count);
        }
    }
}
