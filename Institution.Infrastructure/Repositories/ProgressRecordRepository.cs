using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class ProgressRecordRepository(AppDbContext dbContext) : IProgressRecordRepository
    {
        public async Task AddAsync(ProgressRecordEntity entity) => await dbContext.ProgressRecords.AddAsync(entity);

        public async Task AddRangeAsync(List<ProgressRecordEntity> entities) => await dbContext.ProgressRecords.AddRangeAsync(entities);

        public async Task<List<ProgressRecordEntity>> GetByEditionAsync(Guid programEditionId) =>
            await dbContext.ProgressRecords
                .Include(x => x.Enrollment)
                .Where(x => x.Enrollment.ProgramEditionId == programEditionId)
                .ToListAsync();

        public async Task<ProgressRecordEntity?> FindByEnrollmentAndPeriodAsync(Guid enrollmentId, Guid programPeriodId) =>
            await dbContext.ProgressRecords
                .FirstOrDefaultAsync(x => x.EnrollmentId == enrollmentId && x.ProgramPeriodId == programPeriodId);

        public Task UpdateAsync(ProgressRecordEntity entity)
        {
            dbContext.ProgressRecords.Update(entity);
            return Task.CompletedTask;
        }
    }
}
