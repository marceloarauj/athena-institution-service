using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IProgressRecordRepository
    {
        Task AddAsync(ProgressRecordEntity entity);
        Task AddRangeAsync(List<ProgressRecordEntity> entities);
        Task<List<ProgressRecordEntity>> GetByEditionAsync(Guid programEditionId);
        Task<ProgressRecordEntity?> FindByEnrollmentAndPeriodAsync(Guid enrollmentId, Guid programPeriodId);
        Task UpdateAsync(ProgressRecordEntity entity);
    }
}
