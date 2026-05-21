using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("subject", Schema = Schemes.ACADEMIC)]
    public class SubjectEntity : BaseEntity
    {
        [Column("name"), MaxLength(300)]
        public required string Name { get; set; }

        [Column("code"), MaxLength(30)]
        public required string Code { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("academic_program_id")]
        [ForeignKey(nameof(AcademicProgram))]
        public required Guid AcademicProgramId { get; set; }
        public required AcademicProgramEntity AcademicProgram { get; set; }
    }
}
