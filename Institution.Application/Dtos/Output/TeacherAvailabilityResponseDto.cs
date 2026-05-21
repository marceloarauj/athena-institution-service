using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class TeacherAvailabilityResponseDto
    {
        public Guid Id { get; set; }
        public Guid TeacherId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public Guid ShiftId { get; set; }

        public TeacherAvailabilityResponseDto(TeacherAvailabilityEntity e)
        {
            Id = e.Id;
            TeacherId = e.TeacherId;
            DayOfWeek = e.DayOfWeek;
            ShiftId = e.ShiftId;
        }
    }
}
