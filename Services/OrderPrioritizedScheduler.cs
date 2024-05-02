using speedyairly.Constants;
using speedyairly.Entities;
using speedyairly.Options;

namespace speedyairly.Services
{
    internal class OrderPrioritizedScheduler : IScheduler
    {
        private readonly IOrderProvider _provider;
        private readonly SchedulerOptions _options;

        /// <summary>
        /// Scheduler based on the priority of the order ID
        /// </summary>
        /// <param name="provider"></param>
        public OrderPrioritizedScheduler(IOrderProvider provider, SchedulerOptions options)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public IEnumerable<IFlight> GetSchedules()
        {
            var orders = _provider.GetOrders();
            var flights = new List<IFlight>();
            int nextflightNumber = 1;
            int latestDay = 1;

            foreach (var order in orders)
            {
                var flight = GetOrAddFlight(flights, order.Destination, latestDay, ref nextflightNumber);
                flight.AddOrder(order);
                latestDay = Math.Max(latestDay, flight.Day);
            }
            return flights;
        }

        private IFlight GetOrAddFlight(List<IFlight> flights, Airport destination, int latestDay, ref int nextflightNumber)
        {
            // Use existing flight if the destination matched and it is not full
            var availableFlight = flights.FirstOrDefault(f => f.Arrival == destination && !f.IsFull());
            if (availableFlight != null)
            {
                return availableFlight;
            }

            // Use another flight otherwise
            // Start flight in a new day if no more rooms for the day
            var day = flights.Count(f => f.Day == latestDay) < _options.MaxPlanesPerDay ? latestDay : latestDay + 1;

            // Start a new flight with incremental flight number
            var flight = new Flight(nextflightNumber++, _options.DepartureAirport, destination, day);
            flights.Add(flight);
            return flight;
        }
    }
}
