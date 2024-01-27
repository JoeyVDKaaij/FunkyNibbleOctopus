namespace Game.Items
{
    public interface IItemAcceptor
    {
        public bool IsItemAcceptable (object requester, IItem item);
        public bool AcceptItem (object requester, IItem item);
    }
}
