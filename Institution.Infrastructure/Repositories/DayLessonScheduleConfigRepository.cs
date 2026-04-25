using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class DayLessonScheduleConfigRepository(AppDbContext dbContext) : IDayLessonScheduleConfigRepository
    {
        public async Task AddAsync(DayLessonScheduleConfigEntity config)
        {
            await dbContext.DayLessonScheduleConfigs.AddAsync(config);
        }

        public async Task<DayLessonScheduleConfigEntity?> FindByIdAsync(Guid id)
        {
            return await dbContext.DayLessonScheduleConfigs
                .Include(c => c.Discipline)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<DayLessonScheduleConfigEntity?> FindActiveByDisciplineAsync(Guid disciplineId)
        {
            return await dbContext.DayLessonScheduleConfigs
                .Include(c => c.Discipline)
                .Where(c => c.IsActive && c.DisciplineId == disciplineId)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<DayLessonScheduleConfigEntity?> FindActiveByInstitutionAsync(Guid institutionId)
        {
            return await dbContext.DayLessonScheduleConfigs
                .Where(c => c.IsActive && c.DisciplineId == null && c.InstitutionId == institutionId)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<DayLessonScheduleConfigEntity>> GetByInstitutionAsync(Guid institutionId)
        {
            return await dbContext.DayLessonScheduleConfigs
                .Include(c => c.Discipline)
                .Where(c => c.InstitutionId == institutionId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
