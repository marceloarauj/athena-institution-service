using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("curriculum_entry", Schema = Schemes.ACADEMIC)]
    public class CurriculumEntryEntity : BaseEntity
    {
        [Column("grade_or_year")]
        public int? GradeOrYear { get; set; }

        [Column("period_number")]
        public int? PeriodNumber { get; set; }

        [Column("weekly_hours")]
        public int? WeeklyHours { get; set; }

        [Column("total_hours")]
        public int? TotalHours { get; set; }

        [Column("program_edition_id")]
        [ForeignKey(nameof(ProgramEdition))]
        public required Guid ProgramEditionId { get; set; }
        public required ProgramEditionEntity ProgramEdition { get; set; }

        [Column("subject_id")]
        [ForeignKey(nameof(Subject))]
        public required Guid SubjectId { get; set; }
        public required SubjectEntity Subject { get; set; }
    }
}
