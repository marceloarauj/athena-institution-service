using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("day_lesson_schedule_config_history", Schema = Schemes.INSTITUTION)]
    public class DayLessonScheduleConfigHistoryEntity : BaseEntity
    {
        [Column("updated_by_user")]
        public required Guid UpdatedByUser { get; set; }

        [Column("field"), MaxLength(100)]
        public required string Field { get; set; }

        [Column("old_value"), MaxLength(300)]
        public string? OldValue { get; set; }

        [Column("new_value"), MaxLength(300)]
        public string? NewValue { get; set; }

        [Column("config_id")]
        [ForeignKey(nameof(Config))]
        public required Guid ConfigId { get; set; }
        public required DayLessonScheduleConfigEntity Config { get; set; }
    }
}
