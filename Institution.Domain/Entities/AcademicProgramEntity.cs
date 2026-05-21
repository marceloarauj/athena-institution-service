using Institution.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("academic_program", Schema = Schemes.ACADEMIC)]
    public class AcademicProgramEntity : BaseEntity
    {
        [Column("name"), MaxLength(300)]
        public required string Name { get; set; }

        [Column("type")]
        public required ProgramType Type { get; set; }

        [Column("period_type")]
        public required PeriodType PeriodType { get; set; }

        [Column("has_weekly_schedule")]
        public bool HasWeeklySchedule { get; set; } = false;

        [Column("duration_years")]
        public int DurationYears { get; set; } = 1;

        [Column("min_completion_percent")]
        public decimal? MinCompletionPercent { get; set; }

        [Column("min_school_days")]
        public int? MinSchoolDays { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("institution_id")]
        [ForeignKey(nameof(Institution))]
        public required Guid InstitutionId { get; set; }
        public required InstitutionEntity Institution { get; set; }
    }
}
