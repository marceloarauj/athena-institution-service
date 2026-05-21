using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class ClassScheduleRepository(AppDbContext dbContext) : IClassScheduleRepository
    {
        public async Task AddRangeAsync(List<ClassScheduleEntity> entities) => await dbContext.ClassSchedules.AddRangeAsync(entities);

        public async Task DeleteByEditionAsync(Guid programEditionId)
        {
            var groupIds = await dbContext.ClassGroups
                .Where(x => x.ProgramEditionId == programEditionId)
                .Select(x => x.Id)
                .ToListAsync();

            var existing = await dbContext.ClassSchedules
                .Where(x => groupIds.Contains(x.ClassGroupId))
                .ToListAsync();

            dbContext.ClassSchedules.RemoveRange(existing);
        }

        public async Task<List<ClassScheduleEntity>> GetByGroupAsync(Guid classGroupId) =>
            await dbContext.ClassSchedules
                .Include(x => x.Subject)
                .Include(x => x.Teacher)
                .Include(x => x.ScheduleSlot)
                .Where(x => x.ClassGroupId == classGroupId)
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.ScheduleSlot.Order)
                .ToListAsync();

        public async Task<List<ClassScheduleEntity>> GetByTeacherAsync(Guid teacherId, Guid programEditionId)
        {
            var groupIds = await dbContext.ClassGroups
                .Where(x => x.ProgramEditionId == programEditionId)
                .Select(x => x.Id)
                .ToListAsync();

            return await dbContext.ClassSchedules
                .Include(x => x.Subject)
                .Include(x => x.ClassGroup)
                .Include(x => x.ScheduleSlot)
                .Where(x => x.TeacherId == teacherId && groupIds.Contains(x.ClassGroupId))
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.ScheduleSlot.Order)
                .ToListAsync();
        }

        public async Task<List<ClassScheduleEntity>> GetByEditionAsync(Guid programEditionId)
        {
            var groupIds = await dbContext.ClassGroups
                .Where(x => x.ProgramEditionId == programEditionId)
                .Select(x => x.Id)
                .ToListAsync();

            return await dbContext.ClassSchedules
                .Include(x => x.ClassGroup)
                .Include(x => x.Subject)
                .Include(x => x.Teacher)
                    .ThenInclude(t => t.Availabilities)
                .Include(x => x.ScheduleSlot)
                .Where(x => groupIds.Contains(x.ClassGroupId))
                .ToListAsync();
        }

        public async Task AddLogAsync(ScheduleGenerationLogEntity log) => await dbContext.ScheduleGenerationLogs.AddAsync(log);

        public Task UpdateAsync(ClassScheduleEntity entity)
        {
            dbContext.ClassSchedules.Update(entity);
            return Task.CompletedTask;
        }
    }
}
