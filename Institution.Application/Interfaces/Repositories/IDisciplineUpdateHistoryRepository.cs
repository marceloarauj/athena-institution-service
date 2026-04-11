using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IDisciplineUpdateHistoryRepository
    {
        Task AddRangeAsync(List<DisciplineUpdateHistoryEntity> updateHistories);
    }
}
