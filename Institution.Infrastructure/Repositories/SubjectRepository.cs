using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class SubjectRepository(AppDbContext dbContext) : ISubjectRepository
    {
        public async Task AddAsync(SubjectEntity entity) => await dbContext.Subjects.AddAsync(entity);

        public async Task<SubjectEntity?> FindByIdAsync(Guid id) =>
            await dbContext.Subjects.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<SubjectEntity>> GetByProgramAsync(Guid academicProgramId) =>
            await dbContext.Subjects
                .Where(x => x.AcademicProgramId == academicProgramId)
                .OrderBy(x => x.Name)
                .ToListAsync();
    }
}
