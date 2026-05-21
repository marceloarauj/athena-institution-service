using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class ShiftResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public ShiftResponseDto(ShiftEntity e)
        {
            Id = e.Id;
            Name = e.Name;
            StartTime = e.StartTime;
            EndTime = e.EndTime;
            IsActive = e.IsActive;
            CreatedAt = e.CreatedAt;
        }
    }
}
