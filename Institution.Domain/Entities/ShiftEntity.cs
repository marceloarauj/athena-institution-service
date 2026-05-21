using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("shift", Schema = Schemes.SCHEDULING)]
    public class ShiftEntity : BaseEntity
    {
        [Column("name"), MaxLength(100)]
        public required string Name { get; set; }

        [Column("start_time")]
        public required TimeOnly StartTime { get; set; }

        [Column("end_time")]
        public required TimeOnly EndTime { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("institution_id")]
        [ForeignKey(nameof(Institution))]
        public required Guid InstitutionId { get; set; }
        public required InstitutionEntity Institution { get; set; }

        public ICollection<ScheduleSlotEntity> Slots { get; set; } = [];
    }
}
