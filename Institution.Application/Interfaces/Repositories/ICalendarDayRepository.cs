using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface ICalendarDayRepository
    {
        Task AddRangeAsync(List<CalendarDayEntity> entities);
        Task DeleteByEditionAsync(Guid programEditionId);
        Task<List<CalendarDayEntity>> GetByEditionAsync(Guid programEditionId);
        Task<int> CountSchoolDaysAsync(Guid programEditionId);
    }
}
