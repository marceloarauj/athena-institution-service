using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("class_schedule", Schema = Schemes.SCHEDULING)]
    public class ClassScheduleEntity : BaseEntity
    {
        [Column("day_of_week")]
        public required DayOfWeek DayOfWeek { get; set; }

        [Column("class_group_id")]
        [ForeignKey(nameof(ClassGroup))]
        public required Guid ClassGroupId { get; set; }
        public ClassGroupEntity ClassGroup { get; set; } = null!;

        [Column("subject_id")]
        [ForeignKey(nameof(Subject))]
        public required Guid SubjectId { get; set; }
        public SubjectEntity Subject { get; set; } = null!;

        [Column("teacher_id")]
        [ForeignKey(nameof(Teacher))]
        public required Guid TeacherId { get; set; }
        public TeacherEntity Teacher { get; set; } = null!;

        [Column("schedule_slot_id")]
        [ForeignKey(nameof(ScheduleSlot))]
        public required Guid ScheduleSlotId { get; set; }
        public ScheduleSlotEntity ScheduleSlot { get; set; } = null!;

        [Column("program_period_id")]
        [ForeignKey(nameof(ProgramPeriod))]
        public Guid? ProgramPeriodId { get; set; }
        public ProgramPeriodEntity? ProgramPeriod { get; set; }
    }
}
