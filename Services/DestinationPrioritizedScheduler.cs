using speedyairly.Constants;
using speedyairly.Entities;
using speedyairly.Options;

namespace speedyairly.Services
{
    internal class DestinationPrioritizedScheduler : IScheduler
    {
        private readonly IOrderProvider _orderProvider;
        private readonly SchedulerOptions _options;

        /// <summary>
        /// Scheduler prioritize on destinations so that orders will be distributed evenly to different destinaations.
        /// </summary>
        /// <param name="orderProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DestinationPrioritizedScheduler(IOrderProvider orderProvider, SchedulerOptions options)
        {
            _orderProvider = orderProvider ?? throw new ArgumentNullException(nameof(orderProvider));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public IEnumerable<IFlight> GetSchedules()
        {
            var allOrders = _orderProvider.GetOrders() ?? [];
            var destinationGroups = allOrders.GroupBy(o => o.Destination).ToDictionary(g => g.Key, g => new Queue<IOrder>(g));
            var day = 1;
            var flightNumber = 1;
            var planePool = _options.MaxPlanesPerDay;

            while (destinationGroups.Count != 0)
            {
                var toRemove = new List<Airport>();

                // Prioritize the flights based on the order ID of each destinations
                var prioritizedGroups = GetGroupsOrderedById(destinationGroups);

                foreach (var group in prioritizedGroups)
                {
                    var destination = group.Key;
                    var orders = group.Value;

                    if (planePool == 0)
                    {
                        // Start another day when no more plan available
                        day++;
                        planePool = _options.MaxPlanesPerDay;
                    }

                    var flight = new Flight(flightNumber++, _options.DepartureAirport, destination, day);
                    planePool--;
                    
                    // Fill up the plane for orders in this destination per iteration
                    FillFlight(flight, orders);

                    if (orders.Count == 0)
                    {
                        // No more orders at this destination and hence can be removed.
                        // Add destination to be removed due to enumeration.
                        toRemove.Add(destination);
                    }

                    yield return flight;
                }

                // Clean up empty orders destinations
                RemoveCompletedDestination(destinationGroups, toRemove);
            }
        }

        private static IOrderedEnumerable<KeyValuePair<Airport, Queue<IOrder>>> GetGroupsOrderedById(Dictionary<Airport, Queue<IOrder>> groups)
        {
            return groups.OrderBy(g => g.Value.First().Id);
        }

        private static void FillFlight(IFlight flight, Queue<IOrder> orders)
        {
            while (!flight.IsFull() && orders.Count != 0)
            {
                // Fill the plane as long as the plane is not full and there are orders for this destination
                flight.AddOrder(orders.Dequeue());
            }
        }

        private static void RemoveCompletedDestination(Dictionary<Airport, Queue<IOrder>> destinationGroups, List<Airport> toRemove)
        {
            foreach (var destination in toRemove)
            {
                destinationGroups.Remove(destination);
            }
        }
    }
}
