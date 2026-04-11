using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;

namespace Institution.Infrastructure.Repositories
{
    public class DisciplineUpdateHistoryRepository(AppDbContext dbContext) : IDisciplineUpdateHistoryRepository
    {
        public async Task AddRangeAsync(List<DisciplineUpdateHistoryEntity> updateHistories)
        {
            await dbContext.DisciplineUpdateHistories.AddRangeAsync(updateHistories);
        }
    }
}
