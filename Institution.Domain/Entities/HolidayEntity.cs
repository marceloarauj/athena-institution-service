using Institution.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("holiday", Schema = Schemes.ACADEMIC)]
    public class HolidayEntity : BaseEntity
    {
        [Column("name"), MaxLength(300)]
        public required string Name { get; set; }

        [Column("date")]
        public required DateOnly Date { get; set; }

        [Column("type")]
        public required HolidayType Type { get; set; }

        [Column("is_recurring")]
        public bool IsRecurring { get; set; }

        [Column("institution_id")]
        [ForeignKey(nameof(Institution))]
        public required Guid InstitutionId { get; set; }
        public required InstitutionEntity Institution { get; set; }
    }
}
