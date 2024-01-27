using Game.Items;
using UnityEngine;

namespace Game
{
    public class ItemInteractionScript : MonoBehaviour
    {
        private IItem _currentItem;

        private Transform _childObject;
        private bool _isItemInteractionRequested;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.E))
                _isItemInteractionRequested = true;
            else if (Input.GetKeyUp(KeyCode.JoystickButton0) || Input.GetKeyUp(KeyCode.E))
                _isItemInteractionRequested = false;

            return;

            RaycastHit hit;
            Ray ray = new Ray(transform.position + new Vector3(1,1,0), -transform.up);
            if(Physics.Raycast(ray, out hit))
            {
                if (Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.E))
                {
                    if (hit.collider.CompareTag("Table") && _childObject != null && hit.transform.childCount == 0)
                    {
                        _childObject.SetParent(hit.transform);
                        _childObject.localPosition = new Vector3(0, 1, 0);
                        _childObject = null;
                        _isItemInteractionRequested = true;
                    }
                }
            }

            if (_childObject != null && !_isItemInteractionRequested)
            {
                if (Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.E))
                {
                    _childObject.localPosition = new Vector3(0, 0, 1);
                    _childObject.SetParent(null);
                    _childObject = null;
                    _isItemInteractionRequested = true;
                }
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

        /*
        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.CompareTag("Dish") && transform.childCount <= 1 && !_isItemInteractionRequested && !(other.transform.parent != null))
            {
                if (Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.E))
                {
                    other.transform.SetParent(transform);
                    _childObject = other.transform;
                    _childObject.localPosition = new Vector3(0, 1.2f, 0);
                    _isItemInteractionRequested = true;
                }
            }
            else if (other.gameObject.CompareTag("Table") && _childObject != null && !_isItemInteractionRequested)
            {
                if (Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.E))
                {
                    _childObject.SetParent(other.transform);
                    _childObject.localPosition = new Vector3(0, 1, 0);
                    _childObject = null;
                    _isItemInteractionRequested = true;
                }
            }
        }
        */

        private void HoldItem (IItem item)
        {
            _currentItem = item;
            _currentItem.SetParent(transform, new Vector3(0, 1.2f, 0));
        }
    }
}
