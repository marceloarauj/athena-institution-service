using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IHolidayRepository
    {
        Task AddAsync(HolidayEntity entity);
        Task<HolidayEntity?> FindByIdAsync(Guid id);
        Task<List<HolidayEntity>> GetByYearAsync(Guid institutionId, int? year = null);
        Task DeleteAsync(HolidayEntity entity);
        Task AddRecessAsync(RecessEntity recess);
        Task<List<RecessEntity>> GetRecessesByEditionAsync(Guid programEditionId);
    }
}
