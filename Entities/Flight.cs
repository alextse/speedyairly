using speedyairly.Constants;

namespace speedyairly.Entities
{
    internal interface IAggregateRoot { }

    internal class Flight : IFlight, IAggregateRoot
    {
        public int FlightNumber { get; }
        public Airport Departure { get; }
        public Airport Arrival { get; }
        public int Day { get; }
        public IPlane Plane { get; }

        public Flight(int flightNumber, Airport departure, Airport arrival, int day)
        {
            if (flightNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(flightNumber), $"Flight number cannot be smaller than 1. Value = {flightNumber}");
            }

            if (departure == arrival)
            {
                throw new InvalidOperationException($"Departure city cannot be the same as the arrival city. Value = {Departure}");
            }

            if (day < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(day), $"Day cannot be smaller than 1. Value = {day}");
            }

            FlightNumber = flightNumber;
            Departure = departure;
            Arrival = arrival;
            Day = day;
            Plane = new Plane();
        }

        public bool IsFull()
        {
            return Plane.IsFull();
        }

        public void AddOrder(IOrder order)
        {
            Plane.AddOrder(order);
        }
    }
}
