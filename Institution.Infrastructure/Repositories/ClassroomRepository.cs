using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class ClassroomRepository(AppDbContext dbContext) : IClassroomRepository
    {
        public async Task AddAsync(ClassroomEntity classroom)
        {
            await dbContext.AddAsync(classroom);
        }

        public async Task<List<ClassroomEntity>> GetByFilterAsync(Guid institutionId, Guid? disciplineId, Guid? teacherId, DateTime? startDate, DateTime? endDate)
        {
            return await dbContext.Classrooms
                .Include(classroom => classroom.Discipline)
                .Where(classroom => classroom.Discipline.InstitutionId == institutionId)
                .Where(classroom => disciplineId == null || classroom.DisciplineId == disciplineId)
                .Where(classroom => teacherId == null || classroom.TeacherId == teacherId)
                .Where(classroom => startDate == null || classroom.StartDate >= startDate)
                .Where(classroom => endDate == null || classroom.EndDate <= endDate)
                .OrderBy(classroom => classroom.StartDate)
                .ToListAsync();
        }
    }
}
