namespace Institution.Application.Dtos.Input
{
    public class SetTeacherAvailabilityDto
    {
        public Guid TeacherId { get; set; }
        public required List<TeacherAvailabilityItemDto> Availabilities { get; set; }
    }

    public class TeacherAvailabilityItemDto
    {
        public required DayOfWeek DayOfWeek { get; set; }
        public required Guid ShiftId { get; set; }
    }
}
