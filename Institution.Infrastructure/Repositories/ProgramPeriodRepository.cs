using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class ProgramPeriodRepository(AppDbContext dbContext) : IProgramPeriodRepository
    {
        public async Task AddRangeAsync(List<ProgramPeriodEntity> entities) => await dbContext.ProgramPeriods.AddRangeAsync(entities);

        public async Task AddAsync(ProgramPeriodEntity entity) => await dbContext.ProgramPeriods.AddAsync(entity);

        public async Task<List<ProgramPeriodEntity>> GetByEditionAsync(Guid programEditionId) =>
            await dbContext.ProgramPeriods
                .Where(x => x.ProgramEditionId == programEditionId)
                .OrderBy(x => x.Number)
                .ToListAsync();

        public async Task DeleteByEditionAsync(Guid programEditionId)
        {
            var existing = await dbContext.ProgramPeriods.Where(x => x.ProgramEditionId == programEditionId).ToListAsync();
            dbContext.ProgramPeriods.RemoveRange(existing);
        }

        public Task UpdateSchoolDaysAsync(ProgramPeriodEntity entity)
        {
            dbContext.ProgramPeriods.Update(entity);
            return Task.CompletedTask;
        }
    }
}
