using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("day_lesson", Schema = Schemes.CLASSROOM)]
    public class DayLessonEntity : BaseEntity
    {
        [Column("started")]
        public bool Started { get; set; } = false;

        [Column("location"), MaxLength(300)]
        public required string Location { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("canceled_at")]
        public DateTime? CanceledAt { get; set; }

        [Column("teacher_id")]
        public Guid TeacherId { get; set; }

        [Column("lesson_replacement_id")]
        public Guid? LessonReplacementId { get; set; }

        [Column("classroom_id")]
        [ForeignKey(nameof(Classroom))]
        public Guid? ClassroomId { get; set; }
        public ClassroomEntity? Classroom { get; set; }

        public List<DayLessonDisciplineTopic>? DayLessonDisciplineTopics { get; set; }
        public List<StudentDayLesson>? StudentDayLessons { get; set; }
    }
}
