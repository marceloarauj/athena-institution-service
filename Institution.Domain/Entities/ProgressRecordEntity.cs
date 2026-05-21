using Institution.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("progress_record", Schema = Schemes.ENROLLMENT)]
    public class ProgressRecordEntity : BaseEntity
    {
        [Column("status")]
        public required ProgressStatus Status { get; set; }

        [Column("final_grade")]
        public decimal? FinalGrade { get; set; }

        [Column("completion_percent")]
        public decimal? CompletionPercent { get; set; }

        [Column("enrollment_id")]
        [ForeignKey(nameof(Enrollment))]
        public required Guid EnrollmentId { get; set; }
        public EnrollmentEntity Enrollment { get; set; } = null!;

        [Column("program_period_id")]
        [ForeignKey(nameof(ProgramPeriod))]
        public required Guid ProgramPeriodId { get; set; }
        public ProgramPeriodEntity ProgramPeriod { get; set; } = null!;
    }
}
