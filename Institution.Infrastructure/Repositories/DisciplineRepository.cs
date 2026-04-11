using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;

namespace Institution.Infrastructure.Repositories
{
    public class DisciplineRepository(AppDbContext dbContext) : IDisciplineRepository
    {
        public async Task AddAsync(DisciplineEntity discipline)
        {
            await dbContext.AddAsync(discipline);
        }
    }
}
