using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("discipline_topic", Schema = Schemes.INSTITUTION)]
    public class DisciplineTopicEntity : BaseEntity
    {
        [Column("content"), MaxLength(1000)]
        public required string Content { get; set; }

        [Column("lesson_number")]
        public int LessonNumber { get; set; }

        [ForeignKey(nameof(Discipline))]
        [Column("discipline_id")]
        public Guid DisciplineId { get; set; }

        public DisciplineEntity? Discipline { get; set; }
    }
}
