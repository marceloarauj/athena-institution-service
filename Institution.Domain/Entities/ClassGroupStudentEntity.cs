using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("class_group_student", Schema = Schemes.ENROLLMENT)]
    public class ClassGroupStudentEntity : BaseEntity
    {
        [Column("assigned_at")]
        public required DateTime AssignedAt { get; set; }

        [Column("class_group_id")]
        [ForeignKey(nameof(ClassGroup))]
        public required Guid ClassGroupId { get; set; }
        public required ClassGroupEntity ClassGroup { get; set; }

        [Column("enrollment_id")]
        [ForeignKey(nameof(Enrollment))]
        public required Guid EnrollmentId { get; set; }
        public required EnrollmentEntity Enrollment { get; set; }
    }
}
