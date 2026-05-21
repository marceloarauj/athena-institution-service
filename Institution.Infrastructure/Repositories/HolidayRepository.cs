using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class HolidayRepository(AppDbContext dbContext) : IHolidayRepository
    {
        public async Task AddAsync(HolidayEntity entity) => await dbContext.Holidays.AddAsync(entity);

        public async Task<HolidayEntity?> FindByIdAsync(Guid id) =>
            await dbContext.Holidays.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<HolidayEntity>> GetByYearAsync(Guid institutionId, int? year = null)
        {
            if (year == null)
                return await dbContext.Holidays
                    .Where(x => x.InstitutionId == institutionId && x.IsRecurring)
                    .ToListAsync();

            return await dbContext.Holidays
                .Where(x => x.InstitutionId == institutionId &&
                    (x.IsRecurring || x.Date.Year == year.Value))
                .ToListAsync();
        }

        public Task DeleteAsync(HolidayEntity entity)
        {
            dbContext.Holidays.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task AddRecessAsync(RecessEntity recess) => await dbContext.Recesses.AddAsync(recess);

        public async Task<List<RecessEntity>> GetRecessesByEditionAsync(Guid programEditionId) =>
            await dbContext.Recesses
                .Where(x => x.ProgramEditionId == programEditionId)
                .OrderBy(x => x.StartDate)
                .ToListAsync();
    }
}
