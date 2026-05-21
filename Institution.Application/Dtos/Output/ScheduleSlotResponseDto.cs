using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class ScheduleSlotResponseDto
    {
        public Guid Id { get; set; }
        public Guid ShiftId { get; set; }
        public int Order { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public ScheduleSlotResponseDto(ScheduleSlotEntity e)
        {
            Id = e.Id;
            ShiftId = e.ShiftId;
            Order = e.Order;
            StartTime = e.StartTime;
            EndTime = e.EndTime;
        }
    }
}
