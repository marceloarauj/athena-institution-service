using Institution.Domain.Entities;
using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Output
{
    public class DayLessonScheduleConfigResponseDto(DayLessonScheduleConfigEntity config)
    {
        public Guid Id { get; set; } = config.Id;
        public DateTime StartDate { get; set; } = config.StartDate;
        public DateTime? EndDate { get; set; } = config.EndDate;
        public TimeOnly LessonStartTime { get; set; } = config.LessonStartTime;
        public TimeOnly LessonEndTime { get; set; } = config.LessonEndTime;
        public WeekDays DaysOfWeek { get; set; } = config.DaysOfWeek;
        public int TimesPerWeek { get; set; } = CountSetBits((int)config.DaysOfWeek);
        public int? LessonCount { get; set; } = config.LessonCount;
        public bool IsActive { get; set; } = config.IsActive;
        public Guid InstitutionId { get; set; } = config.InstitutionId;
        public Guid? DisciplineId { get; set; } = config.DisciplineId;
        public string? DisciplineName { get; set; } = config.Discipline?.Name;
        public DateTime CreatedAt { get; set; } = config.CreatedAt;

        private static int CountSetBits(int value)
        {
            var count = 0;
            while (value != 0)
            {
                count += value & 1;
                value >>= 1;
            }
            return count;
        }
    }
}
