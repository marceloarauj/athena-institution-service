using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class ClassGroupRepository(AppDbContext dbContext) : IClassGroupRepository
    {
        public async Task AddAsync(ClassGroupEntity entity) => await dbContext.ClassGroups.AddAsync(entity);

        public async Task AddRangeAsync(List<ClassGroupEntity> entities) => await dbContext.ClassGroups.AddRangeAsync(entities);

        public async Task<ClassGroupEntity?> FindByIdAsync(Guid id) =>
            await dbContext.ClassGroups
                .Include(x => x.Room)
                .Include(x => x.Shift)
                .Include(x => x.Students)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<ClassGroupEntity>> GetByEditionAsync(Guid programEditionId) =>
            await dbContext.ClassGroups
                .Include(x => x.Room)
                .Include(x => x.Shift)
                    .ThenInclude(s => s != null ? s.Slots : null!)
                .Include(x => x.Students)
                .Where(x => x.ProgramEditionId == programEditionId)
                .OrderBy(x => x.GradeOrYear)
                .ThenBy(x => x.Name)
                .ToListAsync();

        public async Task AddStudentsAsync(List<ClassGroupStudentEntity> students) => await dbContext.ClassGroupStudents.AddRangeAsync(students);

        public async Task<List<ClassGroupStudentEntity>> GetStudentsByGroupAsync(Guid classGroupId) =>
            await dbContext.ClassGroupStudents
                .Where(x => x.ClassGroupId == classGroupId)
                .ToListAsync();
    }
}
