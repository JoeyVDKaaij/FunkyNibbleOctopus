using UnityEngine;

namespace Game.Items
{
    public interface IItemProvider
    {
        public bool IsItemAvailable (Vector3 position);
        public IItem GetItem (Vector3 position);
    }
}
