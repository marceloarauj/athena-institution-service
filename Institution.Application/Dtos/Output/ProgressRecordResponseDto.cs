using Institution.Domain.Entities;
using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Output
{
    public class ProgressRecordResponseDto
    {
        public Guid Id { get; set; }
        public Guid EnrollmentId { get; set; }
        public Guid ProgramPeriodId { get; set; }
        public ProgressStatus Status { get; set; }
        public decimal? FinalGrade { get; set; }
        public decimal? CompletionPercent { get; set; }
        public DateTime CreatedAt { get; set; }

        public ProgressRecordResponseDto(ProgressRecordEntity e)
        {
            Id = e.Id;
            EnrollmentId = e.EnrollmentId;
            ProgramPeriodId = e.ProgramPeriodId;
            Status = e.Status;
            FinalGrade = e.FinalGrade;
            CompletionPercent = e.CompletionPercent;
            CreatedAt = e.CreatedAt;
        }
    }
}
