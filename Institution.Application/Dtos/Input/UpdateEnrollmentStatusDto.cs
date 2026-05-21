using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Input
{
    public class UpdateEnrollmentStatusDto
    {
        public required EnrollmentStatus Status { get; set; }
    }
}
