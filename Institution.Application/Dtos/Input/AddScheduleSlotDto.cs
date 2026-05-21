namespace Institution.Application.Dtos.Input
{
    public class AddScheduleSlotDto
    {
        public Guid ShiftId { get; set; }
        public required int Order { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
    }
}
