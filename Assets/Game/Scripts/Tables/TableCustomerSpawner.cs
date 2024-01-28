using UnityEngine;

namespace Game.Tables
{
    public class TableCustomerSpawner : MonoBehaviour
    {
        [SerializeField]
        private TableCustomerSpawnPoint[] spawnPoints;

        [SerializeField]
        private TableCustomer[] customerPrefabs;

        public TableCustomer[] SpawnCustomers ()
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            var spawnPoint = spawnPoints[spawnPointIndex];
            var customerPrefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
            var customer = Object.Instantiate(customerPrefab, spawnPoint.transform.position, Quaternion.identity);
            spawnPoint.SetCustomer(customer);

            if (Random.Range(0f, 1f) < 0.67f)
                return new[] { customer };

            TableCustomer previousCustomer = customer;
            int previousSpawnPointIndex = spawnPointIndex;
            do {
                spawnPointIndex = Random.Range(0, spawnPoints.Length);
            } while (spawnPointIndex == previousSpawnPointIndex);
            spawnPoint = spawnPoints[spawnPointIndex];
            customerPrefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
            customer = Object.Instantiate(customerPrefab, spawnPoint.transform.position, Quaternion.identity);
            spawnPoint.SetCustomer(customer);

            return new[] { previousCustomer, customer };
        }
    }
}
