using speedyairly.Constants;
using speedyairly.Entities;

namespace speedyairly.Services
{
    internal class RandomizedOrderProvider : IOrderProvider
    {
        private readonly Airport[] _arrivalOptions = Enum.GetValues<Airport>().Cast<Airport>().Skip(1).ToArray();

        public IEnumerable<IOrder> GetOrders()
        {
            for (var i = 0; i < 100; i++)
            {
                yield return new Order(i.ToString(), GetRandomArrival());
            }
        }

        private Airport GetRandomArrival()
        {
            var random = new Random();
            return _arrivalOptions.ElementAt(random.Next(0, _arrivalOptions.Length));
        }
    }
}
