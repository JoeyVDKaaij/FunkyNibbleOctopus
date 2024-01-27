using UnityEngine;

public class ItemInteractionScript : MonoBehaviour
{
    private Transform _childObject;
    private bool _buttonPressed;
    
    void Update()
    {
        if (Input.GetKey(KeyCode.JoystickButton0) && _childObject != null && !_buttonPressed)
        {
            _childObject.localPosition = new Vector3(0, 0, 1);
            _childObject.SetParent(null);
            _childObject = null;
            _buttonPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.JoystickButton0)) _buttonPressed = false;
    }

    private void OnCollisionStay(Collision other)
    {
        if (Input.GetKey(KeyCode.JoystickButton0) && other.gameObject.CompareTag("Dish") && transform.childCount <= 1 && !_buttonPressed)
        {
            other.transform.SetParent(transform);
            _childObject = other.transform;
            _childObject.localPosition = new Vector3(0, 1, 0);
            _buttonPressed = true;
        }
    }
}
