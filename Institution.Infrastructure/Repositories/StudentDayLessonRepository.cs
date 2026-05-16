using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class StudentDayLessonRepository(AppDbContext dbContext) : IStudentDayLessonRepository
    {
        public async Task<List<StudentDayLesson>> GetByDayLessonIdAsync(Guid dayLessonId)
        {
            return await dbContext.StudentDayLessons
                .Where(dayLesson => dayLesson.DayLessonId == dayLessonId)
                .ToListAsync();
        }

        public async Task<StudentDayLesson?> FindByDayLessonAndStudentAsync(Guid dayLessonId, Guid studentId)
        {
            return await dbContext.StudentDayLessons
                .FirstOrDefaultAsync(studentDayLesson => studentDayLesson.DayLessonId == dayLessonId && studentDayLesson.StudentId == studentId);
        }

        public async Task AddRangeAsync(List<StudentDayLesson> records)
        {
            await dbContext.StudentDayLessons.AddRangeAsync(records);
        }
    }
}
