using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Input
{
    public class BulkRecordProgressDto
    {
        public Guid ProgramEditionId { get; set; }
        public required Guid ProgramPeriodId { get; set; }
        public required List<RecordProgressItemDto> Records { get; set; }
    }

    public class RecordProgressItemDto
    {
        public required Guid EnrollmentId { get; set; }
        public required ProgressStatus Status { get; set; }
        public decimal? FinalGrade { get; set; }
        public decimal? CompletionPercent { get; set; }
    }
}
