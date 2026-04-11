using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IInstitutionUpdateHistoryRepository
    {
       Task AddRangeAsync(List<InstitutionUpdateHistoryEntity> updateHistories); 
    }
}