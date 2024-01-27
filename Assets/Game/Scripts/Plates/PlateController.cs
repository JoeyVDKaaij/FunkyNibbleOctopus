using Game.Items;
using UnityEngine;
using Zenject;

namespace Game.Plates
{
    public class PlateController : MonoBehaviour, IItem
    {
        [Inject]
        private PlatesController _platesController;

        public Plate Plate { get; private set; }

        public void SetType (PlateType type)
        {
            Plate = Plate.Create(type);
        }

        public void SetParent (Transform parent, Vector3 offset)
        {
            transform.SetParent(parent);
            transform.localPosition = offset;
        }
    }
}
