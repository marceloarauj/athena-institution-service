using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;

namespace Institution.Infrastructure.Repositories
{
    public class InstitutionUpdateHistoryRepository(AppDbContext dbContext) : IInstitutionUpdateHistoryRepository
    {
        public async Task AddRangeAsync(List<InstitutionUpdateHistoryEntity> updateHistories)
        {
            await dbContext.InstitutionUpdateHistories.AddRangeAsync(updateHistories);
        }
    }
}