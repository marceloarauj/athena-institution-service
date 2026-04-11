using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IEventRepository
    {
        Task AddAsync(EventEntity ev);
    }
}
