using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("discipline_update_history", Schema = Schemes.INSTITUTION)]
    public class DisciplineUpdateHistoryEntity : BaseEntity
    {
        [Column("updated_by_user")]
        public required Guid UpdatedByUser { get; set; }

        [Column("field"), MaxLength(100)]
        public required string Field { get; set; }

        [Column("old_value"), MaxLength(300)]
        public required string OldValue { get; set; }

        [Column("new_value"), MaxLength(300)]
        public required string NewValue { get; set; }

        [Column("discipline_id")]
        [ForeignKey(nameof(Discipline))]
        public Guid DisciplineId { get; set; }
        public required DisciplineEntity Discipline { get; set; }
    }
}
