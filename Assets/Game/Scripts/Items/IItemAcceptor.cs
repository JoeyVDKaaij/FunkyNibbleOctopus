using UnityEngine;

namespace Game.Items
{
    public interface IItemAcceptor
    {
        public bool IsItemAcceptable (Vector3 position, IItem item);
        public bool AcceptItem (Vector3 position, IItem item);
    }
}
