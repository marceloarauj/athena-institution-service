using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class ConflictReportRepository(AppDbContext dbContext) : IConflictReportRepository
    {
        public async Task AddAsync(ConflictReportEntity entity) => await dbContext.ConflictReports.AddAsync(entity);

        public async Task<ConflictReportEntity?> GetLatestByEditionAsync(Guid programEditionId) =>
            await dbContext.ConflictReports
                .Include(x => x.Items)
                .Where(x => x.ProgramEditionId == programEditionId)
                .OrderByDescending(x => x.GeneratedAt)
                .FirstOrDefaultAsync();

        public async Task DeleteByEditionAsync(Guid programEditionId)
        {
            var reports = await dbContext.ConflictReports
                .Include(x => x.Items)
                .Where(x => x.ProgramEditionId == programEditionId)
                .ToListAsync();

            foreach (var report in reports)
                dbContext.ConflictItems.RemoveRange(report.Items);

            dbContext.ConflictReports.RemoveRange(reports);
        }
    }
}
