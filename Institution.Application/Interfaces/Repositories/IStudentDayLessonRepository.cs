using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IStudentDayLessonRepository
    {
        Task<List<StudentDayLesson>> GetByDayLessonIdAsync(Guid dayLessonId);
        Task AddRangeAsync(List<StudentDayLesson> records);
    }
}
