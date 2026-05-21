using Institution.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("schedule_generation_log", Schema = Schemes.SCHEDULING)]
    public class ScheduleGenerationLogEntity : BaseEntity
    {
        [Column("generated_at")]
        public required DateTime GeneratedAt { get; set; }

        [Column("status")]
        public required GenerationStatus Status { get; set; }

        [Column("total_assigned")]
        public required int TotalAssigned { get; set; }

        [Column("total_unresolved")]
        public required int TotalUnresolved { get; set; }

        [Column("program_edition_id")]
        [ForeignKey(nameof(ProgramEdition))]
        public required Guid ProgramEditionId { get; set; }
        public required ProgramEditionEntity ProgramEdition { get; set; }
    }
}
