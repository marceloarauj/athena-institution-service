using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("teacher_availability", Schema = Schemes.SCHEDULING)]
    public class TeacherAvailabilityEntity : BaseEntity
    {
        [Column("teacher_id")]
        [ForeignKey(nameof(Teacher))]
        public required Guid TeacherId { get; set; }
        public required TeacherEntity Teacher { get; set; }

        [Column("day_of_week")]
        public required DayOfWeek DayOfWeek { get; set; }

        [Column("shift_id")]
        [ForeignKey(nameof(Shift))]
        public required Guid ShiftId { get; set; }
        public required ShiftEntity Shift { get; set; }
    }
}
