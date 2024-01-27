namespace Game.Items
{
    public interface IItemProvider
    {
        public bool IsItemAvailable (object requester);
        public IItem GetItem (object requester);
    }
}
