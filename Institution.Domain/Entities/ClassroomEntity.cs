using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("classroom", Schema = Schemes.CLASSROOM)]
    public class ClassroomEntity : BaseEntity
    {
        [Column("max_students")]
        public int? MaxStudents { get; set; }

        [Column("location"), MaxLength(300)]
        public required string Location { get; set; }

        [Column("teacher_id")]
        public required Guid TeacherId { get; set; }

        [Column("start_date")]
        public required DateTime StartDate { get; set; }

        [Column("end_date")]
        public required DateTime EndDate { get; set; }

        [Column("created_by_user")]
        public required Guid CreateByUserId { get; set; }

        [Column("discipline_id")]
        [ForeignKey(nameof(Discipline))]
        public required Guid DisciplineId { get; set; }
        public required DisciplineEntity Discipline { get; set; }
    }
}
