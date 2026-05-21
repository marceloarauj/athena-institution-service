using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class CalendarDayRepository(AppDbContext dbContext) : ICalendarDayRepository
    {
        public async Task AddRangeAsync(List<CalendarDayEntity> entities) => await dbContext.CalendarDays.AddRangeAsync(entities);

        public async Task DeleteByEditionAsync(Guid programEditionId)
        {
            var existing = await dbContext.CalendarDays.Where(x => x.ProgramEditionId == programEditionId).ToListAsync();
            dbContext.CalendarDays.RemoveRange(existing);
        }

        public async Task<List<CalendarDayEntity>> GetByEditionAsync(Guid programEditionId) =>
            await dbContext.CalendarDays
                .Where(x => x.ProgramEditionId == programEditionId)
                .OrderBy(x => x.Date)
                .ToListAsync();

        public async Task<int> CountSchoolDaysAsync(Guid programEditionId) =>
            await dbContext.CalendarDays
                .CountAsync(x => x.ProgramEditionId == programEditionId && x.Type == CalendarDayType.SchoolDay);
    }
}
