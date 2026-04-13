using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class StudentRegistrationResponseDto(StudentClassroomRegistrationEntity registration)
    {
        public Guid Id { get; set; } = registration.Id;
        public Guid StudentId { get; set; } = registration.StudentId;
        public string StudentName { get; set; } = registration.StudentName;
        public bool IsActive { get; set; } = registration.IsActive;
        public Guid ClassroomId { get; set; } = registration.ClassroomId;
        public DateTime CreatedAt { get; set; } = registration.CreatedAt;
    }
}
