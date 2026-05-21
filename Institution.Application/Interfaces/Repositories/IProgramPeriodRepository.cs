using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IProgramPeriodRepository
    {
        Task AddRangeAsync(List<ProgramPeriodEntity> entities);
        Task AddAsync(ProgramPeriodEntity entity);
        Task<List<ProgramPeriodEntity>> GetByEditionAsync(Guid programEditionId);
        Task DeleteByEditionAsync(Guid programEditionId);
        Task UpdateSchoolDaysAsync(ProgramPeriodEntity entity);
    }
}
