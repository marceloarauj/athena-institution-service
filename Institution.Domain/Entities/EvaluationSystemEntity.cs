using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("evaluation_system", Schema = Schemes.INSTITUTION)]
    public class EvaluationSystemEntity : BaseEntity
    {
        [Column("description"), MaxLength(500)]
        public required string Description { get; set; }

        [Column("formula"), MaxLength(300)]
        public required string Formula { get; set; }

        [Column("active")]
        public bool Active { get; set; } = true;

        [Column("discipline_id")]
        [ForeignKey(nameof(Discipline))]
        public Guid DisciplineId { get; set; }

        [Column("institution_id")]
        [ForeignKey(nameof(Institution))]
        public Guid InstitutionId { get; set; }

        public InstitutionEntity? Institution { get; set; }
        public DisciplineEntity? Discipline { get; set; }
    }
}
