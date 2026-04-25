using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class DayLessonRepository(AppDbContext dbContext) : IDayLessonRepository
    {
        public async Task AddRangeAsync(List<DayLessonEntity> dayLessons)
        {
            await dbContext.DayLessons.AddRangeAsync(dayLessons);
        }

        public async Task<DayLessonEntity?> FindByIdAsync(Guid id)
        {
            return await dbContext.DayLessons.FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
