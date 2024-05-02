using speedyairly.Constants;

namespace speedyairly.Entities
{
    internal interface IFlight
    {
        int FlightNumber { get; }
        Airport Departure { get; }
        Airport Arrival { get; }
        int Day { get; }
        IPlane Plane { get; }

        bool IsFull();
        void AddOrder(IOrder order);
    }
}
