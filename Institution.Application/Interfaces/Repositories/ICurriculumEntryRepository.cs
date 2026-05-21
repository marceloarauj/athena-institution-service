using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface ICurriculumEntryRepository
    {
        Task UpsertAsync(CurriculumEntryEntity entry);
        Task BulkUpsertAsync(List<CurriculumEntryEntity> entries);
        Task<List<CurriculumEntryEntity>> GetByEditionAsync(Guid programEditionId, int? gradeOrYear = null);
    }
}
