using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IRoomRepository
    {
        Task AddAsync(RoomEntity entity);
        Task<RoomEntity?> FindByIdAsync(Guid id);
        Task<List<RoomEntity>> GetByInstitutionAsync(Guid institutionId);
    }
}
