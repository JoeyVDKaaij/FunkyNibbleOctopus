using Game.Items;
using UnityEngine;
using Zenject;

namespace Game.Plates
{
    public class PlatesPickupController : MonoInstaller, IItemProvider, IItemAcceptor
    {
        private PlateSpawnPoint[] _plateSpawnPoints;

        public override void InstallBindings ()
        {
            Container.Bind<PlatesPickupController>().FromInstance(this).AsSingle().NonLazy();
        }

        public void SetSpawnPoints (PlateSpawnPoint[] plateSpawnPoints)
        {
            _plateSpawnPoints = plateSpawnPoints;
        }

        public bool IsItemAvailable (Vector3 position)
        {
            (_, bool found) = GetClosestSpawnPoint(position);

            return found;
        }

        public IItem GetItem (Vector3 position)
        {
            return GivePlate(position);
        }

        public PlateController GivePlate (Vector3 position)
        {
            (var spawnPoint, bool found) = GetClosestSpawnPoint(position);
            if (!found)
                return null;

            var plate = spawnPoint.Plate;
            spawnPoint.Plate = null;

            return plate;
        }

        public bool IsItemAcceptable (Vector3 position, IItem item)
        {
            if (item is not PlateController)
                return false;

            (_, bool found) = GetClosestFreeSpawnPoint(position);

            return found;
        }

        public bool AcceptItem (Vector3 position, IItem item)
        {
            if (item is PlateController plate)
                return AcceptPlate(position, plate);

            return false;
        }

        public bool AcceptPlate (Vector3 position, PlateController plate)
        {
            (var spawnPoint, bool found) = GetClosestFreeSpawnPoint(position);
            if (!found)
                return false;

            spawnPoint.Plate = plate;
            plate.SetParent(spawnPoint.transform, Vector3.zero);

            return true;
        }

        private (PlateSpawnPoint, bool) GetClosestSpawnPoint (Vector3 position)
        {
            PlateSpawnPoint closestSpawnPoint = null;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < _plateSpawnPoints.Length; i++) {
                if (_plateSpawnPoints[i].Plate == null)
                    continue;

                float distance = Vector3.Distance(position, _plateSpawnPoints[i].transform.position);
                if (distance < closestDistance) {
                    closestSpawnPoint = _plateSpawnPoints[i];
                    closestDistance = distance;
                }
            }

            return (closestSpawnPoint, closestDistance != float.MaxValue);
        }

        private (PlateSpawnPoint, bool) GetClosestFreeSpawnPoint (Vector3 position)
        {
            PlateSpawnPoint closestSpawnPoint = null;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < _plateSpawnPoints.Length; i++) {
                if (_plateSpawnPoints[i].Plate != null)
                    continue;

                float distance = Vector3.Distance(position, _plateSpawnPoints[i].transform.position);
                if (distance < closestDistance) {
                    closestSpawnPoint = _plateSpawnPoints[i];
                    closestDistance = distance;
                }
            }

            return (closestSpawnPoint, closestDistance != float.MaxValue);
        }
    }
}
