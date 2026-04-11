using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("student_classroom_registration", Schema = Schemes.CLASSROOM)]
    public class StudentClassroomRegistrationEntity : BaseEntity
    {
        [Column("student_id")]
        public Guid StudentId { get; set; }

        [Column("student_name"), MaxLength(1000)]
        public required string StudentName { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("updated_by")]
        public DateTime? UpdatedBy { get; set; }

        [Column("classroom_id")]
        [ForeignKey(nameof(Classroom))]
        public Guid ClassroomId { get; set; }
        public required ClassroomEntity Classroom { get; set; }
    }
}
