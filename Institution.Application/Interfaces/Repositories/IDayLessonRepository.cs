using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IDayLessonRepository
    {
        Task AddRangeAsync(List<DayLessonEntity> dayLessons);
    }
}
