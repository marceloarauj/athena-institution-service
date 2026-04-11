using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class DisciplineRepository(AppDbContext dbContext) : IDisciplineRepository
    {
        public async Task AddAsync(DisciplineEntity discipline)
        {
            await dbContext.AddAsync(discipline);
        }

        public async Task<DisciplineEntity?> FindByIdAsync(Guid id)
        {
            return await dbContext.Disciplines.FirstOrDefaultAsync(discipline => discipline.Id == id);
        }
    }
}
