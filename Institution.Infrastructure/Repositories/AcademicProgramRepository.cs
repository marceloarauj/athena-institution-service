using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class AcademicProgramRepository(AppDbContext dbContext) : IAcademicProgramRepository
    {
        public async Task AddAsync(AcademicProgramEntity entity) => await dbContext.AcademicPrograms.AddAsync(entity);

        public async Task<AcademicProgramEntity?> FindByIdAsync(Guid id) =>
            await dbContext.AcademicPrograms.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<AcademicProgramEntity>> GetByInstitutionAsync(Guid institutionId) =>
            await dbContext.AcademicPrograms
                .Where(x => x.InstitutionId == institutionId)
                .OrderBy(x => x.Name)
                .ToListAsync();
    }
}
