using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class StudentClassroomRegistrationRepository(AppDbContext dbContext) : IStudentClassroomRegistrationRepository
    {
        public async Task AddAsync(StudentClassroomRegistrationEntity registration)
        {
            await dbContext.AddAsync(registration);
        }

        public async Task<bool> ExistsAsync(Guid studentId, Guid classroomId)
        {
            return await dbContext.StudentClassroomRegistrations
                .AnyAsync(r => r.StudentId == studentId && r.ClassroomId == classroomId);
        }

        public async Task<StudentClassroomRegistrationEntity?> FindByStudentAndClassroomAsync(Guid studentId, Guid classroomId)
        {
            return await dbContext.StudentClassroomRegistrations
                .FirstOrDefaultAsync(registry => registry.StudentId == studentId && registry.ClassroomId == classroomId);
        }

        public async Task<List<StudentClassroomRegistrationEntity>> GetActiveByStudentIdsAsync(List<Guid> studentIds, Guid classroomId)
        {
            return await dbContext.StudentClassroomRegistrations
                .Where(registry => registry.ClassroomId == classroomId && registry.IsActive && studentIds.Contains(registry.StudentId))
                .ToListAsync();
        }
    }
}
