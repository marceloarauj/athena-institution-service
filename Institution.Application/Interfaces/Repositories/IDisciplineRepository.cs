using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IDisciplineRepository
    {
        Task AddAsync(DisciplineEntity discipline);
        Task<DisciplineEntity?> FindByIdAsync(Guid id);
    }
}
