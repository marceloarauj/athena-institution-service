using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class InstitutionRepository(AppDbContext dbContext) : IInstitutionRepository
    {
        public async Task AddAsync(InstitutionEntity institution)
        {
            await dbContext.AddAsync(institution);
        }

        public async Task<bool> ExistsByAliasAsync(string alias)
        {
            return await dbContext.Institutions.Where(institution => institution.Alias == alias).AnyAsync();
        }

        public async Task<InstitutionEntity?> FindByAliasAsync(string alias)
        {
            return await dbContext.Institutions.Where(institution => institution.Alias == alias).SingleOrDefaultAsync();
        }
    }
}