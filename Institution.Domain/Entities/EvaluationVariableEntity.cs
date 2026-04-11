using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("evaluation_variable", Schema = Schemes.INSTITUTION)]
    [Index(nameof(InstitutionId), nameof(Key), IsUnique = true)]
    public class EvaluationVariableEntity : BaseEntity
    {
        [Column("key"), MaxLength(30)]
        public required string Key { get; set; }

        [Column("description"), MaxLength(500)]
        public required string Description { get; set; }
        
        [Column("institution_id")]
        [ForeignKey(nameof(InstitutionId))]
        public Guid InstitutionId { get; set; }

        public required InstitutionEntity Institution { get; set; }
    }
}
