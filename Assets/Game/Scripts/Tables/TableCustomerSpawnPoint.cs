using UnityEngine;

namespace Game.Tables
{
    public class TableCustomerSpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private bool flip;
        
        [SerializeField]
        private Vector3 rotationAxis = Vector3.up;

        public void SetCustomer (TableCustomer customer)
        {
            customer.transform.SetParent(transform);
            customer.transform.localPosition = Vector3.zero;
            customer.transform.localRotation = Quaternion.
                LookRotation(flip ? -transform.forward : transform.forward, rotationAxis);
        }
    }
}
