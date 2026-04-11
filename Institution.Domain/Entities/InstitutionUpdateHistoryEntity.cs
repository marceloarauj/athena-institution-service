using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("institution_update_history", Schema = Schemes.INSTITUTION)]
    public class InstitutionUpdateHistoryEntity : BaseEntity
    {
        [Column("updated_by_user")]
        public required Guid UpdatedByUser { get; set; }

        [Column("field"), MaxLength(50)]
        public required string Field { get; set; }

        [Column("old_value"), MaxLength(300)]
        public string? OldValue { get; set; }

        [Column("new_value"), MaxLength(300)]
        public string? NewValue { get; set; }

        [Column("institution_id")]
        [ForeignKey(nameof(Institution))]
        public Guid InstitutionId { get; set; }

        public required InstitutionEntity Institution { get; set; }
    }
}
