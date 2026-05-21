using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("conflict_report", Schema = Schemes.ENROLLMENT)]
    public class ConflictReportEntity : BaseEntity
    {
        [Column("generated_at")]
        public required DateTime GeneratedAt { get; set; }

        [Column("total_critical")]
        public required int TotalCritical { get; set; }

        [Column("total_high")]
        public required int TotalHigh { get; set; }

        [Column("total_medium")]
        public required int TotalMedium { get; set; }

        [Column("program_edition_id")]
        [ForeignKey(nameof(ProgramEdition))]
        public required Guid ProgramEditionId { get; set; }
        public required ProgramEditionEntity ProgramEdition { get; set; }

        public ICollection<ConflictItemEntity> Items { get; set; } = [];
    }
}
