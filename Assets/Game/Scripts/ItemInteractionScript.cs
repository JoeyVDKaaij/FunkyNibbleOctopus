using System;
using UnityEngine;

public class ItemInteractionScript : MonoBehaviour
{
    private Transform _childObject;
    private bool _buttonPressed;
    
    
    void Update()
    {
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
                    _buttonPressed = true;
                }
            }
        }
        
        if (_childObject != null && !_buttonPressed)
        {
            if (Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.E))
            {
                _childObject.localPosition = new Vector3(0, 0, 1);
                _childObject.SetParent(null);
                _childObject = null;
                _buttonPressed = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.JoystickButton0) || Input.GetKeyUp(KeyCode.E)) _buttonPressed = false;
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Dish") && transform.childCount <= 1 && !_buttonPressed && !(other.transform.parent != null))
        {
            if (Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.E))
            {
                other.transform.SetParent(transform);
                _childObject = other.transform;
                _childObject.localPosition = new Vector3(0, 1.2f, 0);
                _buttonPressed = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name);
        
    }
    
}
