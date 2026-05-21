using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IClassScheduleRepository
    {
        Task AddRangeAsync(List<ClassScheduleEntity> entities);
        Task DeleteByEditionAsync(Guid programEditionId);
        Task<List<ClassScheduleEntity>> GetByGroupAsync(Guid classGroupId);
        Task<List<ClassScheduleEntity>> GetByTeacherAsync(Guid teacherId, Guid programEditionId);
        Task<List<ClassScheduleEntity>> GetByEditionAsync(Guid programEditionId);
        Task AddLogAsync(ScheduleGenerationLogEntity log);
        Task UpdateAsync(ClassScheduleEntity entity);
    }
}
