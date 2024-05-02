namespace speedyairly.Entities
{
    internal interface IPlane
    {
        IReadOnlyList<IOrder> Orders { get; }
        bool IsFull();
        void AddOrder(IOrder order);
    }
}
