using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class TeacherResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public TeacherResponseDto(TeacherEntity e)
        {
            Id = e.Id;
            UserId = e.UserId;
            Name = e.Name;
            Email = e.Email;
            IsActive = e.IsActive;
            CreatedAt = e.CreatedAt;
        }
    }
}
