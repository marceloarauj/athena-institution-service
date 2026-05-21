namespace Institution.Application.Dtos.Input
{
    public class OverrideScheduleSlotDto
    {
        public required Guid ClassGroupId { get; set; }
        public required Guid SubjectId { get; set; }
        public required Guid TeacherId { get; set; }
        public required DayOfWeek DayOfWeek { get; set; }
        public required Guid ScheduleSlotId { get; set; }
    }
}
