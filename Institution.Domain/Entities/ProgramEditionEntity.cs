using Institution.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("program_edition", Schema = Schemes.ACADEMIC)]
    public class ProgramEditionEntity : BaseEntity
    {
        [Column("name"), MaxLength(300)]
        public required string Name { get; set; }

        [Column("start_date")]
        public required DateOnly StartDate { get; set; }

        [Column("end_date")]
        public required DateOnly EndDate { get; set; }

        [Column("status")]
        public EditionStatus Status { get; set; } = EditionStatus.Draft;

        [Column("published_at")]
        public DateTime? PublishedAt { get; set; }

        [Column("academic_program_id")]
        [ForeignKey(nameof(AcademicProgram))]
        public required Guid AcademicProgramId { get; set; }
        public required AcademicProgramEntity AcademicProgram { get; set; }
    }
}
