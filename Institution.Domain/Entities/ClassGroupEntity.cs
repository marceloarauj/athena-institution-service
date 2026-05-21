using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("class_group", Schema = Schemes.ENROLLMENT)]
    public class ClassGroupEntity : BaseEntity
    {
        [Column("name"), MaxLength(100)]
        public required string Name { get; set; }

        [Column("grade_or_year")]
        public int? GradeOrYear { get; set; }

        [Column("max_students")]
        public required int MaxStudents { get; set; }

        [Column("program_edition_id")]
        [ForeignKey(nameof(ProgramEdition))]
        public required Guid ProgramEditionId { get; set; }
        public required ProgramEditionEntity ProgramEdition { get; set; }

        [Column("room_id")]
        [ForeignKey(nameof(Room))]
        public Guid? RoomId { get; set; }
        public RoomEntity? Room { get; set; }

        [Column("shift_id")]
        [ForeignKey(nameof(Shift))]
        public Guid? ShiftId { get; set; }
        public ShiftEntity? Shift { get; set; }

        public ICollection<ClassGroupStudentEntity> Students { get; set; } = [];
    }
}
