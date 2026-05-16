using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class DayLessonRepository(AppDbContext dbContext) : IDayLessonRepository
    {
        public async Task AddRangeAsync(List<DayLessonEntity> dayLessons)
        {
            await dbContext.DayLessons.AddRangeAsync(dayLessons);
        }

        public async Task<DayLessonEntity?> FindByIdAsync(Guid id)
        {
            return await dbContext.DayLessons.FirstOrDefaultAsync(dayLesson => dayLesson.Id == id);
        }

        public async Task<List<DayLessonEntity>> GetByClassroomIdAsync(Guid classroomId)
        {
            return await dbContext.DayLessons
                .Include(dayLesson => dayLesson.DayLessonDisciplineTopics!)
                    .ThenInclude(topic => topic.DisciplineTopic)
                .Include(dayLesson => dayLesson.StudentDayLessons)
                .Where(dayLesson => dayLesson.ClassroomId == classroomId)
                .OrderBy(dayLesson => dayLesson.StartDate)
                .ToListAsync();
        }
    }
}
