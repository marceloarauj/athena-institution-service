using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class RoomRepository(AppDbContext dbContext) : IRoomRepository
    {
        public async Task AddAsync(RoomEntity entity) => await dbContext.Rooms.AddAsync(entity);

        public async Task<RoomEntity?> FindByIdAsync(Guid id) =>
            await dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<RoomEntity>> GetByInstitutionAsync(Guid institutionId) =>
            await dbContext.Rooms
                .Where(x => x.InstitutionId == institutionId)
                .OrderBy(x => x.Name)
                .ToListAsync();
    }
}
