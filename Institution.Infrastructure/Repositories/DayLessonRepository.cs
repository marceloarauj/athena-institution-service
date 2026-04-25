using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;

namespace Institution.Infrastructure.Repositories
{
    public class DayLessonRepository(AppDbContext dbContext) : IDayLessonRepository
    {
        public async Task AddRangeAsync(List<DayLessonEntity> dayLessons)
        {
            await dbContext.DayLessons.AddRangeAsync(dayLessons);
        }
    }
}
