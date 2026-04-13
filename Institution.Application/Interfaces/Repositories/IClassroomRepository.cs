using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IClassroomRepository
    {
        Task AddAsync(ClassroomEntity classroom);
        Task<ClassroomEntity?> FindByIdAsync(Guid id);
        Task<List<ClassroomEntity>> GetByFilterAsync(Guid institutionId, Guid? disciplineId, Guid? teacherId, DateTime? startDate, DateTime? endDate);
    }
}
