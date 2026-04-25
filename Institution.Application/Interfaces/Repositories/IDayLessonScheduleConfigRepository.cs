using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IDayLessonScheduleConfigRepository
    {
        Task AddAsync(DayLessonScheduleConfigEntity config);
        Task<DayLessonScheduleConfigEntity?> FindByIdAsync(Guid id);
        Task<DayLessonScheduleConfigEntity?> FindActiveByDisciplineAsync(Guid disciplineId);
        Task<DayLessonScheduleConfigEntity?> FindActiveByInstitutionAsync(Guid institutionId);
        Task<List<DayLessonScheduleConfigEntity>> GetByInstitutionAsync(Guid institutionId);
    }
}
