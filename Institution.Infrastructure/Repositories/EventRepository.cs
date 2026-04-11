using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class EventRepository(AppDbContext dbContext) : IEventRepository
    {
        public async Task AddAsync(EventEntity @event)
        {
            await dbContext.AddAsync(@event);
        }

        public async Task<List<EventEntity>> GetByFilterAsync(Guid institutionId, string? name, DateTime startDate, DateTime endDate)
        {
            return await dbContext.Events
                .Where(@event => @event.InstitutionId == institutionId)
                .Where(@event => @event.StartDate <= endDate && @event.EndDate >= startDate)
                .Where(@event => name == null || @event.Name.Contains(name))
                .OrderBy(@event => @event.StartDate)
                .ToListAsync();
        }
    }
}
