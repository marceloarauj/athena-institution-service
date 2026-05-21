using Institution.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("enrollment", Schema = Schemes.ENROLLMENT)]
    public class EnrollmentEntity : BaseEntity
    {
        [Column("student_id")]
        public required Guid StudentId { get; set; }

        [Column("grade_or_year")]
        public int? GradeOrYear { get; set; }

        [Column("status")]
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

        [Column("enrolled_at")]
        public required DateTime EnrolledAt { get; set; }

        [Column("purchase_reference"), MaxLength(300)]
        public string? PurchaseReference { get; set; }

        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        [Column("program_edition_id")]
        [ForeignKey(nameof(ProgramEdition))]
        public required Guid ProgramEditionId { get; set; }
        public ProgramEditionEntity ProgramEdition { get; set; } = null!;
    }
}
