namespace speedyairly.Entities
{
    internal class Plane : IPlane
    {
        private const int MaxOrder = 20;
        private readonly List<IOrder> _orders = [];

        public IReadOnlyList<IOrder> Orders => _orders.AsReadOnly();

        public bool IsFull()
        {
            return _orders.Count >= MaxOrder;
        }

        public void AddOrder(IOrder order)
        {
            ArgumentNullException.ThrowIfNull(order);

            if (IsFull())
            {
                throw new InvalidOperationException($"A flight cannot take more than {MaxOrder} orders.");
            }

            _orders.Add(order);
        }
    }
}
