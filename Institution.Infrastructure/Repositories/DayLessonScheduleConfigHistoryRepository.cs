using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;

namespace Institution.Infrastructure.Repositories
{
    public class DayLessonScheduleConfigHistoryRepository(AppDbContext dbContext) : IDayLessonScheduleConfigHistoryRepository
    {
        public async Task AddRangeAsync(List<DayLessonScheduleConfigHistoryEntity> histories)
        {
            await dbContext.DayLessonScheduleConfigHistories.AddRangeAsync(histories);
        }
    }
}
