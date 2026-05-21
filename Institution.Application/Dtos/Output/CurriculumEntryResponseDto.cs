using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class CurriculumEntryResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProgramEditionId { get; set; }
        public Guid SubjectId { get; set; }
        public int? GradeOrYear { get; set; }
        public int? PeriodNumber { get; set; }
        public int? WeeklyHours { get; set; }
        public int? TotalHours { get; set; }

        public CurriculumEntryResponseDto(CurriculumEntryEntity e)
        {
            Id = e.Id;
            ProgramEditionId = e.ProgramEditionId;
            SubjectId = e.SubjectId;
            GradeOrYear = e.GradeOrYear;
            PeriodNumber = e.PeriodNumber;
            WeeklyHours = e.WeeklyHours;
            TotalHours = e.TotalHours;
        }
    }
}
