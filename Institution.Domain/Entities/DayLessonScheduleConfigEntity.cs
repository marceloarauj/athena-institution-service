using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Institution.Domain.Enums;

namespace Institution.Domain.Entities
{
    [Table("day_lesson_schedule_config", Schema = Schemes.INSTITUTION)]
    public class DayLessonScheduleConfigEntity : BaseEntity
    {
        [Column("start_date")]
        public required DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("lesson_start_time")]
        public required TimeOnly LessonStartTime { get; set; }

        [Column("lesson_end_time")]
        public required TimeOnly LessonEndTime { get; set; }

        [Column("days_of_week")]
        public required WeekDays DaysOfWeek { get; set; }

        [Column("lesson_count")]
        public int? LessonCount { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("institution_id")]
        [ForeignKey(nameof(Institution))]
        public required Guid InstitutionId { get; set; }
        public required InstitutionEntity Institution { get; set; }

        [Column("discipline_id")]
        [ForeignKey(nameof(Discipline))]
        public Guid? DisciplineId { get; set; }
        public DisciplineEntity? Discipline { get; set; }
    }
}
