using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("student_day_lesson", Schema = Schemes.CLASSROOM)]
    public class StudentDayLesson : BaseEntity
    {
        [Column("student_id")]
        public required Guid StudentId { get; set; }

        [Column("is_present")]
        public bool? IsPresent { get; set; }

        [Column("observation"), MaxLength(1000)]
        public string? Observations { get; set; }

        [Column("day_lesson_id")]
        [ForeignKey(nameof(DayLesson))]
        public required Guid DayLessonId { get; set; }
        public DayLessonEntity? DayLesson { get; set; }
    }
}
