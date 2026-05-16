using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class DayLessonResponseDto(DayLessonEntity dayLesson)
    {
        public Guid Id { get; set; } = dayLesson.Id;
        public DateTime StartDate { get; set; } = dayLesson.StartDate;
        public DateTime EndDate { get; set; } = dayLesson.EndDate;
        public DateTime? CanceledAt { get; set; } = dayLesson.CanceledAt;
        public List<string> Topics { get; set; } = dayLesson.DayLessonDisciplineTopics?
            .Where(t => t.DisciplineTopic != null)
            .Select(t => t.DisciplineTopic!.Content)
            .ToList() ?? [];
        public List<StudentDayLessonResponseDto> Students { get; set; } = dayLesson.StudentDayLessons?
            .Select(s => new StudentDayLessonResponseDto(s))
            .ToList() ?? [];
    }
}
