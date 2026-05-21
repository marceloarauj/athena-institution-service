using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class ProgramEditionRepository(AppDbContext dbContext) : IProgramEditionRepository
    {
        public async Task AddAsync(ProgramEditionEntity entity) => await dbContext.ProgramEditions.AddAsync(entity);

        public async Task<ProgramEditionEntity?> FindByIdAsync(Guid id) =>
            await dbContext.ProgramEditions
                .Include(x => x.AcademicProgram)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<ProgramEditionEntity>> GetByProgramAsync(Guid academicProgramId) =>
            await dbContext.ProgramEditions
                .Where(x => x.AcademicProgramId == academicProgramId)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();

        public Task UpdateStatusAsync(ProgramEditionEntity entity)
        {
            dbContext.ProgramEditions.Update(entity);
            return Task.CompletedTask;
        }
    }
}
