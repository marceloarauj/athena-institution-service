using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("student_classroom_note", Schema = Schemes.CLASSROOM)]
    [Index(nameof(StudentId), nameof(ClassroomId), nameof(Key), IsUnique = true)]
    public class StudentClassroomNoteEntity : BaseEntity
    {
        [Column("student_id")]
        public required Guid StudentId { get; set; }

        [Column("key"), MaxLength(30)]
        public required string Key { get; set; }

        [Column("value")]
        public required decimal Value { get; set; }

        [Column("classroom_id")]
        [ForeignKey(nameof(Classroom))]
        public required Guid ClassroomId { get; set; }

        public ClassroomEntity? Classroom { get; set; }
    }
}
