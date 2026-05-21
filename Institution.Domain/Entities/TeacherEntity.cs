using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("teacher", Schema = Schemes.SCHEDULING)]
    public class TeacherEntity : BaseEntity
    {
        [Column("user_id")]
        public required Guid UserId { get; set; }

        [Column("name"), MaxLength(300)]
        public required string Name { get; set; }

        [Column("email"), MaxLength(300)]
        public required string Email { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("institution_id")]
        [ForeignKey(nameof(Institution))]
        public required Guid InstitutionId { get; set; }
        public required InstitutionEntity Institution { get; set; }

        public ICollection<TeacherSubjectEntity> Subjects { get; set; } = [];
        public ICollection<TeacherAvailabilityEntity> Availabilities { get; set; } = [];
    }
}
