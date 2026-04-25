using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IDayLessonScheduleConfigHistoryRepository
    {
        Task AddRangeAsync(List<DayLessonScheduleConfigHistoryEntity> histories);
    }
}
