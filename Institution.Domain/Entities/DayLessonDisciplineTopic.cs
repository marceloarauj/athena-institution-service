using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("day_lesson_discipline_topic", Schema = Schemes.CLASSROOM)]
    public class DayLessonDisciplineTopic : BaseEntity
    {
        [Column("day_lesson_id")]
        [ForeignKey(nameof(DayLesson))]
        public Guid DayLessonId { get; set; }

        [Column("discipline_topic_id")]
        [ForeignKey(nameof(DisciplineTopic))]
        public Guid DisciplineTopicId { get; set; }

        public DayLessonEntity? DayLesson { get; set; }
        public DisciplineTopicEntity? DisciplineTopic { get; set; }
    }
}
