using Institution.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("calendar_day", Schema = Schemes.ACADEMIC)]
    public class CalendarDayEntity : BaseEntity
    {
        [Column("date")]
        public required DateOnly Date { get; set; }

        [Column("type")]
        public required CalendarDayType Type { get; set; }

        [Column("holiday_name"), MaxLength(300)]
        public string? HolidayName { get; set; }

        [Column("program_edition_id")]
        [ForeignKey(nameof(ProgramEdition))]
        public required Guid ProgramEditionId { get; set; }
        public required ProgramEditionEntity ProgramEdition { get; set; }

        [Column("program_period_id")]
        [ForeignKey(nameof(ProgramPeriod))]
        public Guid? ProgramPeriodId { get; set; }
        public ProgramPeriodEntity? ProgramPeriod { get; set; }
    }
}
