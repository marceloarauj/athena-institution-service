using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("program_period", Schema = Schemes.ACADEMIC)]
    public class ProgramPeriodEntity : BaseEntity
    {
        [Column("number")]
        public required int Number { get; set; }

        [Column("name"), MaxLength(200)]
        public required string Name { get; set; }

        [Column("start_date")]
        public required DateOnly StartDate { get; set; }

        [Column("end_date")]
        public required DateOnly EndDate { get; set; }

        [Column("school_days")]
        public int? SchoolDays { get; set; }

        [Column("program_edition_id")]
        [ForeignKey(nameof(ProgramEdition))]
        public required Guid ProgramEditionId { get; set; }
        public required ProgramEditionEntity ProgramEdition { get; set; }
    }
}
