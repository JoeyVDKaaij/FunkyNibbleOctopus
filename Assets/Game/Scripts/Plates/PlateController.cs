using Game.Items;
using UnityEngine;
using Zenject;

namespace Game.Plates
{
    public class PlateController : MonoBehaviour, IItem
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [Inject]
        private PlatesRepository _platesRepository;

        public Plate Plate { get; private set; }

        public void SetType (string type)
        {
            Plate = Plate.Create(type);
            spriteRenderer.sprite = _platesRepository.GetByType(type).Sprite;
        }

        public void SetParent (Transform parent, Vector3 offset)
        {
            transform.SetParent(parent);
            transform.localPosition = offset;
            transform.localRotation = Quaternion.identity;
        }
    }
}
