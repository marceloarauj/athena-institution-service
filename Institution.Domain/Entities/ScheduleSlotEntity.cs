using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("schedule_slot", Schema = Schemes.SCHEDULING)]
    public class ScheduleSlotEntity : BaseEntity
    {
        [Column("order")]
        public required int Order { get; set; }

        [Column("start_time")]
        public required TimeOnly StartTime { get; set; }

        [Column("end_time")]
        public required TimeOnly EndTime { get; set; }

        [Column("shift_id")]
        [ForeignKey(nameof(Shift))]
        public required Guid ShiftId { get; set; }
        public required ShiftEntity Shift { get; set; }
    }
}
