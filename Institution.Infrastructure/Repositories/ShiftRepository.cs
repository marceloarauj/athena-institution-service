using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class ShiftRepository(AppDbContext dbContext) : IShiftRepository
    {
        public async Task AddAsync(ShiftEntity entity) => await dbContext.Shifts.AddAsync(entity);

        public async Task<ShiftEntity?> FindByIdAsync(Guid id) =>
            await dbContext.Shifts
                .Include(x => x.Slots)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<ShiftEntity>> GetByInstitutionAsync(Guid institutionId) =>
            await dbContext.Shifts
                .Where(x => x.InstitutionId == institutionId)
                .OrderBy(x => x.Name)
                .ToListAsync();

        public async Task AddSlotAsync(ScheduleSlotEntity slot) => await dbContext.ScheduleSlots.AddAsync(slot);

        public async Task<List<ScheduleSlotEntity>> GetSlotsByShiftAsync(Guid shiftId) =>
            await dbContext.ScheduleSlots
                .Where(x => x.ShiftId == shiftId)
                .OrderBy(x => x.Order)
                .ToListAsync();
    }
}
