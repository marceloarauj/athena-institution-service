using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class ClassGroupStudentResponseDto
    {
        public Guid Id { get; set; }
        public Guid ClassGroupId { get; set; }
        public Guid EnrollmentId { get; set; }
        public DateTime AssignedAt { get; set; }

        public ClassGroupStudentResponseDto(ClassGroupStudentEntity e)
        {
            Id = e.Id;
            ClassGroupId = e.ClassGroupId;
            EnrollmentId = e.EnrollmentId;
            AssignedAt = e.AssignedAt;
        }
    }
}
