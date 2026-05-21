using Institution.Domain.Entities;
using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Output
{
    public class EnrollmentResponseDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid ProgramEditionId { get; set; }
        public int? GradeOrYear { get; set; }
        public EnrollmentStatus Status { get; set; }
        public DateTime EnrolledAt { get; set; }
        public string? PurchaseReference { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public EnrollmentResponseDto(EnrollmentEntity e)
        {
            Id = e.Id;
            StudentId = e.StudentId;
            ProgramEditionId = e.ProgramEditionId;
            GradeOrYear = e.GradeOrYear;
            Status = e.Status;
            EnrolledAt = e.EnrolledAt;
            PurchaseReference = e.PurchaseReference;
            ExpiresAt = e.ExpiresAt;
        }
    }
}
