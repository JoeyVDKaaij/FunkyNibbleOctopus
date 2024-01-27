using System;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [Header("Movement Settings")] 
    [SerializeField, Tooltip("Set the movement speed of the GameObject."), Min(0.1f)]
    private float movementSpeed = 10f;

    private Rigidbody rb;

    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        rb.AddForce(new Vector3(horizontalInput, 0, verticalInput).normalized * movementSpeed);
    }
}
