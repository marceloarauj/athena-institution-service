using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class TeacherRepository(AppDbContext dbContext) : ITeacherRepository
    {
        public async Task AddAsync(TeacherEntity entity) => await dbContext.Teachers.AddAsync(entity);

        public async Task<TeacherEntity?> FindByIdAsync(Guid id) =>
            await dbContext.Teachers
                .Include(x => x.Subjects)
                .Include(x => x.Availabilities)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<TeacherEntity>> GetByInstitutionAsync(Guid institutionId) =>
            await dbContext.Teachers
                .Where(x => x.InstitutionId == institutionId)
                .OrderBy(x => x.Name)
                .ToListAsync();

        public async Task SetSubjectsAsync(Guid teacherId, List<TeacherSubjectEntity> subjects)
        {
            var existing = await dbContext.TeacherSubjects.Where(x => x.TeacherId == teacherId).ToListAsync();
            dbContext.TeacherSubjects.RemoveRange(existing);
            await dbContext.TeacherSubjects.AddRangeAsync(subjects);
        }

        public async Task SetAvailabilityAsync(Guid teacherId, List<TeacherAvailabilityEntity> availabilities)
        {
            var existing = await dbContext.TeacherAvailabilities.Where(x => x.TeacherId == teacherId).ToListAsync();
            dbContext.TeacherAvailabilities.RemoveRange(existing);
            await dbContext.TeacherAvailabilities.AddRangeAsync(availabilities);
        }

        public async Task<List<TeacherAvailabilityEntity>> GetAvailabilityAsync(Guid teacherId) =>
            await dbContext.TeacherAvailabilities
                .Where(x => x.TeacherId == teacherId)
                .ToListAsync();
    }
}
