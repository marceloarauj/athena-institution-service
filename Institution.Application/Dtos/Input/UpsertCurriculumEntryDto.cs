namespace Institution.Application.Dtos.Input
{
    public class UpsertCurriculumEntryDto
    {
        public Guid ProgramEditionId { get; set; }
        public required Guid SubjectId { get; set; }
        public int? GradeOrYear { get; set; }
        public int? PeriodNumber { get; set; }
        public int? WeeklyHours { get; set; }
        public int? TotalHours { get; set; }
    }
}
