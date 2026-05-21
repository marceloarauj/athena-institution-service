using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("room", Schema = Schemes.SCHEDULING)]
    public class RoomEntity : BaseEntity
    {
        [Column("name"), MaxLength(300)]
        public required string Name { get; set; }

        [Column("capacity")]
        public required int Capacity { get; set; }

        [Column("has_lab")]
        public bool HasLab { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("institution_id")]
        [ForeignKey(nameof(Institution))]
        public required Guid InstitutionId { get; set; }
        public required InstitutionEntity Institution { get; set; }
    }
}
