using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Input
{
    public class RecordProgressDto
    {
        public required Guid EnrollmentId { get; set; }
        public required Guid ProgramPeriodId { get; set; }
        public required ProgressStatus Status { get; set; }
        public decimal? FinalGrade { get; set; }
        public decimal? CompletionPercent { get; set; }
    }
}
