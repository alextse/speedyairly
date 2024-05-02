using speedyairly.Constants;

namespace speedyairly.Options
{
    internal class SchedulerOptions
    {
        public int MaxPlanesPerDay { get; set; } = 3;
        public Airport DepartureAirport { get; set; } = Airport.YUL;
    }
}
