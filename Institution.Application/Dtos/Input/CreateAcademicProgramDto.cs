using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Input
{
    public class CreateAcademicProgramDto
    {
        public required string Name { get; set; }
        public required ProgramType Type { get; set; }
        public required PeriodType PeriodType { get; set; }
        public bool HasWeeklySchedule { get; set; } = false;
        public int DurationYears { get; set; } = 1;
        public decimal? MinCompletionPercent { get; set; }
        public int? MinSchoolDays { get; set; }
    }
}
