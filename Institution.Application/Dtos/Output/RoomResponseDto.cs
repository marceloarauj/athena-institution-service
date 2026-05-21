using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class RoomResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool HasLab { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public RoomResponseDto(RoomEntity e)
        {
            Id = e.Id;
            Name = e.Name;
            Capacity = e.Capacity;
            HasLab = e.HasLab;
            IsActive = e.IsActive;
            CreatedAt = e.CreatedAt;
        }
    }
}
