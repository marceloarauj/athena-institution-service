using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class ReportCardLayoutRepository(AppDbContext dbContext) : IReportCardLayoutRepository
    {
        public async Task<ReportCardLayoutEntity?> FindByInstitutionIdAsync(Guid institutionId)
            => await dbContext.ReportCardLayouts.FirstOrDefaultAsync(r => r.InstitutionId == institutionId);

        public async Task AddAsync(ReportCardLayoutEntity layout)
            => await dbContext.AddAsync(layout);

        public void Update(ReportCardLayoutEntity layout)
            => dbContext.Update(layout);
    }
}
