using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface ITeacherRepository
    {
        Task AddAsync(TeacherEntity entity);
        Task<TeacherEntity?> FindByIdAsync(Guid id);
        Task<List<TeacherEntity>> GetByInstitutionAsync(Guid institutionId);
        Task SetSubjectsAsync(Guid teacherId, List<TeacherSubjectEntity> subjects);
        Task SetAvailabilityAsync(Guid teacherId, List<TeacherAvailabilityEntity> availabilities);
        Task<List<TeacherAvailabilityEntity>> GetAvailabilityAsync(Guid teacherId);
    }
}
