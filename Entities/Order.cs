using speedyairly.Constants;

namespace speedyairly.Entities
{
    internal class Order : IOrder
    {
        public string Id { get; }
        public Airport Destination { get; }

        public Order(string id, Airport destination)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));
            }

            Id = id;
            Destination = destination;
        }
    }
}
