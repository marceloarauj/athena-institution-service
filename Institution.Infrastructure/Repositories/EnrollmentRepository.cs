using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class EnrollmentRepository(AppDbContext dbContext) : IEnrollmentRepository
    {
        public async Task AddAsync(EnrollmentEntity entity) => await dbContext.Enrollments.AddAsync(entity);

        public async Task AddRangeAsync(List<EnrollmentEntity> entities) => await dbContext.Enrollments.AddRangeAsync(entities);

        public async Task<EnrollmentEntity?> FindByIdAsync(Guid id) =>
            await dbContext.Enrollments.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> ExistsAsync(Guid studentId, Guid programEditionId) =>
            await dbContext.Enrollments.AnyAsync(x => x.StudentId == studentId && x.ProgramEditionId == programEditionId);

        public async Task<List<EnrollmentEntity>> GetByEditionAsync(Guid programEditionId) =>
            await dbContext.Enrollments
                .Where(x => x.ProgramEditionId == programEditionId)
                .OrderBy(x => x.GradeOrYear)
                .ToListAsync();

        public async Task<List<EnrollmentEntity>> GetByEditionAndGradeAsync(Guid programEditionId, int? gradeOrYear) =>
            await dbContext.Enrollments
                .Where(x => x.ProgramEditionId == programEditionId && x.GradeOrYear == gradeOrYear)
                .ToListAsync();

        public Task UpdateStatusAsync(EnrollmentEntity entity)
        {
            dbContext.Enrollments.Update(entity);
            return Task.CompletedTask;
        }
    }
}
