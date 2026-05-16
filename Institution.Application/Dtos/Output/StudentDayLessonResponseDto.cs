using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class StudentDayLessonResponseDto(StudentDayLesson record)
    {
        public Guid StudentId { get; set; } = record.StudentId;
        public bool? IsPresent { get; set; } = record.IsPresent;
        public string? Observations { get; set; } = record.Observations;
    }
}
