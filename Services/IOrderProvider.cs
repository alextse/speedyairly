using speedyairly.Entities;

namespace speedyairly.Services
{
    internal interface IOrderProvider
    {
        IEnumerable<IOrder> GetOrders();
    }
}
