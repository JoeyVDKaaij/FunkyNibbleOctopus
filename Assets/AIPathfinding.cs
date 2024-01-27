using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIPathfinding : MonoBehaviour
{
    [Header("AI Pathfinding Settings")] [SerializeField, Tooltip("The tables that this GameObject has to pay attention to.")]
    private Transform[] tables = null;
    [SerializeField, Tooltip("The dishes that this GameObject has to pay attention to.")]
    private Transform[] dishes = null;

    private float focusDishId = 0;
    private Transform _childObject;
    
    private NavMeshAgent agent = null;
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent != null && tables != null)
        {
            int focusedTableId = -1;
            for (int i = 0; i < tables.Length; i++)
            {
                if (tables[i].transform.GetChild(0) != null && false) focusedTableId = i;
            }
            
            if (dishes != null && _childObject == null)
            {
                agent.SetDestination(tables[focusedTableId].position);
            }
            else if (_childObject != null && focusedTableId >= 0)
            {
                agent.SetDestination(tables[focusedTableId].position);
            }
            
            else Debug.LogError("Agent component is not active.");
        }
        else Debug.LogError("Cannot find tables.");
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.transform.CompareTag("Dish"))
        {
            other.transform.SetParent(transform);
            _childObject = other.transform;
            _childObject.localPosition = new Vector3(0, 1.2f, 0);
        }
    }
}
