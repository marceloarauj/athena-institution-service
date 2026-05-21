using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IShiftRepository
    {
        Task AddAsync(ShiftEntity entity);
        Task<ShiftEntity?> FindByIdAsync(Guid id);
        Task<List<ShiftEntity>> GetByInstitutionAsync(Guid institutionId);
        Task AddSlotAsync(ScheduleSlotEntity slot);
        Task<List<ScheduleSlotEntity>> GetSlotsByShiftAsync(Guid shiftId);
    }
}
