using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IEventRepository
    {
        Task AddAsync(EventEntity @event);
        Task<List<EventEntity>> GetByFilterAsync(Guid institutionId, string? name, DateTime startDate, DateTime endDate);
    }
}
