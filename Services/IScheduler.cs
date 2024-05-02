using speedyairly.Entities;

namespace speedyairly.Services
{
    internal interface IScheduler
    {
        IEnumerable<IFlight> GetSchedules();
    }
}
