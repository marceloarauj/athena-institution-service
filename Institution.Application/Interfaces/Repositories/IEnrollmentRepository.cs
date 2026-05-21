using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IEnrollmentRepository
    {
        Task AddAsync(EnrollmentEntity entity);
        Task AddRangeAsync(List<EnrollmentEntity> entities);
        Task<EnrollmentEntity?> FindByIdAsync(Guid id);
        Task<bool> ExistsAsync(Guid studentId, Guid programEditionId);
        Task<List<EnrollmentEntity>> GetByEditionAsync(Guid programEditionId);
        Task<List<EnrollmentEntity>> GetByEditionAndGradeAsync(Guid programEditionId, int? gradeOrYear);
        Task UpdateStatusAsync(EnrollmentEntity entity);
    }
}
