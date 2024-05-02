using speedyairly.Entities;
using speedyairly.Options;
using speedyairly.Services;
namespace speedyairly;

class Program
{
    static void Main(string[] args)
    {
        //var provider = new RandomizedOrderProvider();
        var options = new SchedulerOptions();
        var provider = new JsonOrderProvider("coding-assigment-orders.json");

        PrintOrderPrioritizedResults(provider, options);
        WriteSeparator();
        PrintDestinationPrioritizedResults(provider, options);
    }

    private static void PrintDestinationPrioritizedResults(IOrderProvider provider, SchedulerOptions options)
    {
        var scheduler = new DestinationPrioritizedScheduler(provider, options);
        var flights = scheduler.GetSchedules();
        PrintFlights(flights);
        PrintOrders(flights);
    }

    private static void PrintOrderPrioritizedResults(IOrderProvider provider, SchedulerOptions options)
    {
        var scheduler = new OrderPrioritizedScheduler(provider, options);
        var flights = scheduler.GetSchedules();
        PrintFlights(flights);
        PrintOrders(flights);
    }

    private static void WriteSeparator()
    {
        Console.WriteLine(new string('-', Console.WindowWidth));
    }

    private static void PrintFlights(IEnumerable<IFlight> flights)
    {
        foreach (var flight in flights)
        {
            Console.WriteLine($"Flight: {flight.FlightNumber}, departure: {flight.Departure}, arrival: {flight.Arrival}, day: {flight.Day}");
        }
    }

    private static void PrintOrders(IEnumerable<IFlight> flights)
    {
        var orders = flights.SelectMany(f => f.Plane.Orders.Select(o => new
        {
            Flights = f,
            Order = o
        })).OrderBy(o => o.Order.Id);

        foreach (var order in orders)
        {
            Console.WriteLine($"order: {order.Order.Id}, flightNumber: {order.Flights.FlightNumber}, departure: {order.Flights.Departure}, arrival: {order.Flights.Arrival}, day: {order.Flights.Day}");
        }
    }
}
