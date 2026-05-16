using Institution.Domain.Entities;

namespace Institution.Application.Interfaces.Repositories
{
    public interface IStudentDayLessonRepository
    {
        Task<List<StudentDayLesson>> GetByDayLessonIdAsync(Guid dayLessonId);
        Task<StudentDayLesson?> FindByDayLessonAndStudentAsync(Guid dayLessonId, Guid studentId);
        Task AddRangeAsync(List<StudentDayLesson> records);
    }
}
