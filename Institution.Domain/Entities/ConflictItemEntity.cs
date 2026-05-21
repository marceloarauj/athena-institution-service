using Institution.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("conflict_item", Schema = Schemes.ENROLLMENT)]
    public class ConflictItemEntity : BaseEntity
    {
        [Column("code"), MaxLength(50)]
        public required string Code { get; set; }

        [Column("severity")]
        public required ConflictSeverity Severity { get; set; }

        [Column("description"), MaxLength(1000)]
        public required string Description { get; set; }

        [Column("class_group_id")]
        public Guid? ClassGroupId { get; set; }

        [Column("teacher_id")]
        public Guid? TeacherId { get; set; }

        [Column("subject_id")]
        public Guid? SubjectId { get; set; }

        [Column("day_of_week")]
        public DayOfWeek? DayOfWeek { get; set; }

        [Column("schedule_slot_id")]
        public Guid? ScheduleSlotId { get; set; }

        [Column("conflict_report_id")]
        [ForeignKey(nameof(ConflictReport))]
        public Guid ConflictReportId { get; set; }
        public ConflictReportEntity ConflictReport { get; set; } = null!;
    }
}
