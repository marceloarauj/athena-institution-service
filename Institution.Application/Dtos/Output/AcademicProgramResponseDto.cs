using Institution.Domain.Entities;
using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Output
{
    public class AcademicProgramResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ProgramType Type { get; set; }
        public PeriodType PeriodType { get; set; }
        public bool HasWeeklySchedule { get; set; }
        public int DurationYears { get; set; }
        public decimal? MinCompletionPercent { get; set; }
        public int? MinSchoolDays { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public AcademicProgramResponseDto(AcademicProgramEntity e)
        {
            Id = e.Id;
            Name = e.Name;
            Type = e.Type;
            PeriodType = e.PeriodType;
            HasWeeklySchedule = e.HasWeeklySchedule;
            DurationYears = e.DurationYears;
            MinCompletionPercent = e.MinCompletionPercent;
            MinSchoolDays = e.MinSchoolDays;
            IsActive = e.IsActive;
            CreatedAt = e.CreatedAt;
        }
    }
}
