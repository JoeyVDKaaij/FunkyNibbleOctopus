using Game.Plates;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Tables
{
    public class TableDisplay : MonoBehaviour
    {
        [SerializeField]
        private GameObject display;

        [SerializeField]
        private Image image;

        [Inject]
        private PlatesRepository _platesRepository;

        public Plate Plate { get; private set; }

        public void ShowPlate (string type)
        {
            Plate = Plate.Create(type);
            image.sprite = _platesRepository.GetByType(type).Sprite;
            display.SetActive(true);
        }

        public void Hide ()
        {
            display.SetActive(false);
        }
    }
}
