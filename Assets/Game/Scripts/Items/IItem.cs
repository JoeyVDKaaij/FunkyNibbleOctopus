using UnityEngine;

namespace Game.Items
{
    public interface IItem
    {
        public void SetParent (Transform parent, Vector3 offset);
    }
}
