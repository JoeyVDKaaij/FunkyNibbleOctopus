using System;
using UnityEngine;

public class ItemInteractionScript : MonoBehaviour
{
    private Transform childObject;
    
    void Update()
    {
        if (Input.GetKey(KeyCode.JoystickButton0) && childObject != null)
        {
            childObject.localPosition = new Vector3(0, 0, 1);
            childObject.SetParent(null);
            childObject = null;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && other.gameObject.CompareTag("Dish") && transform.childCount <= 1)
        {
            other.transform.SetParent(transform);
            childObject = other.transform;
            childObject.localPosition = new Vector3(0, 1, 0);
        }
    }
}
