using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;

namespace Institution.Infrastructure.Repositories
{
    public class EventRepository(AppDbContext dbContext) : IEventRepository
    {
        public async Task AddAsync(EventEntity @event)
        {
            await dbContext.AddAsync(@event);
        }
    }
}
