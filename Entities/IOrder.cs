using speedyairly.Constants;

namespace speedyairly.Entities
{
    internal interface IOrder
    {
        string Id { get; }
        Airport Destination { get; }
    }
}
