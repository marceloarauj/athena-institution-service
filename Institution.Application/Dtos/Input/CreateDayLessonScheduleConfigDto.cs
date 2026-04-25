using Institution.Domain.Entities;
using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Input
{
    public class CreateDayLessonScheduleConfigDto
    {
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public required TimeOnly LessonStartTime { get; set; }
        public required TimeOnly LessonEndTime { get; set; }
        public required WeekDays DaysOfWeek { get; set; }
        public int? LessonCount { get; set; }
        public Guid? DisciplineId { get; set; }

        public DayLessonScheduleConfigEntity ToEntity(InstitutionEntity institution, DisciplineEntity? discipline)
        {
            return new DayLessonScheduleConfigEntity
            {
                StartDate = StartDate,
                EndDate = EndDate,
                LessonStartTime = LessonStartTime,
                LessonEndTime = LessonEndTime,
                DaysOfWeek = DaysOfWeek,
                LessonCount = LessonCount,
                IsActive = true,
                InstitutionId = institution.Id,
                Institution = institution,
                DisciplineId = discipline?.Id,
                Discipline = discipline
            };
        }
    }
}
