using UnityEngine;

namespace Game.Plates
{
    [CreateAssetMenu(fileName = "PlateContainer", menuName = "Plates/Container", order = 0)]
    public class PlateContainer : ScriptableObject
    {
        [SerializeField]
        private string type;
        public string Type => type;

        [SerializeField]
        private Sprite sprite;
        public Sprite Sprite => sprite;
    }
}
