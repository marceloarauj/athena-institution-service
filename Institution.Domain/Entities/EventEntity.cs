using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("event", Schema = Schemes.INSTITUTION)]
    public class EventEntity : BaseEntity
    {
        [Column("name"), MaxLength(200)]
        public required string Name { get; set; }

        [Column("description"), MaxLength(3000)]
        public string? Description { get; set; }

        [Column("start_date")]
        public required DateTime StartDate { get; set; }

        [Column("end_date")]
        public required DateTime EndDate { get; set; }

        [Column("institution_id")]
        [ForeignKey(nameof(InstitutionId))]
        public Guid InstitutionId { get; set; }

        public required InstitutionEntity Institution { get; set; }
    }
}
