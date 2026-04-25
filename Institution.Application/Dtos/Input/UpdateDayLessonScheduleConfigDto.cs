using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Input
{
    public class UpdateDayLessonScheduleConfigDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeOnly? LessonStartTime { get; set; }
        public TimeOnly? LessonEndTime { get; set; }
        public WeekDays? DaysOfWeek { get; set; }
        public int? LessonCount { get; set; }
        public bool? IsActive { get; set; }
    }
}
