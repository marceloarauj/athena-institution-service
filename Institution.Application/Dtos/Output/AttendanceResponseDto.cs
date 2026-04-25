using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class AttendanceResponseDto(StudentDayLesson record)
    {
        public Guid Id { get; set; } = record.Id;
        public Guid StudentId { get; set; } = record.StudentId;
        public bool? IsPresent { get; set; } = record.IsPresent;
        public string? Observation { get; set; } = record.Observations;
        public Guid DayLessonId { get; set; } = record.DayLessonId;
    }
}
