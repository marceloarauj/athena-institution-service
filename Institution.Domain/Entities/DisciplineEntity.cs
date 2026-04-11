using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("discipline", Schema = Schemes.INSTITUTION)]
    public class DisciplineEntity : BaseEntity
    {
        [Column("name"), MaxLength(300)]
        public required string Name { get; set; }

        [Column("study_hours")]
        public required int StudyHours { get; set; }

        [Column("credits")]
        public required int Credits { get; set; }

        [Column("available")]
        public required bool Available { get; set; }

        [Column("charge_payment")]
        public required bool ChargePayment { get; set; }

        [Column("created_by")]
        public required Guid CreatedBy { get; set; }

        [Column("institution_id")]
        [ForeignKey(nameof(Institution))]
        public required Guid InstitutionId { get; set; }
        public required InstitutionEntity Institution { get; set; }
    }
}
