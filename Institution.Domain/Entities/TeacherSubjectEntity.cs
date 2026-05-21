using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("teacher_subject", Schema = Schemes.SCHEDULING)]
    public class TeacherSubjectEntity : BaseEntity
    {
        [Column("teacher_id")]
        [ForeignKey(nameof(Teacher))]
        public required Guid TeacherId { get; set; }
        public required TeacherEntity Teacher { get; set; }

        [Column("subject_id")]
        [ForeignKey(nameof(Subject))]
        public required Guid SubjectId { get; set; }
        public required SubjectEntity Subject { get; set; }
    }
}
