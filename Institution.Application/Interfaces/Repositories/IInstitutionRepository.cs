using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IInstitutionRepository
    {
        Task AddAsync(InstitutionEntity institution);
        Task<InstitutionEntity?> FindByAliasAsync(string alias);
        Task<bool> ExistsByAliasAsync(string alias);
    }
}
