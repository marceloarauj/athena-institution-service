using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IConflictReportRepository
    {
        Task AddAsync(ConflictReportEntity entity);
        Task<ConflictReportEntity?> GetLatestByEditionAsync(Guid programEditionId);
        Task DeleteByEditionAsync(Guid programEditionId);
    }
}
