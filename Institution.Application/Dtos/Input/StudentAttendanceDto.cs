namespace Institution.Application.Dtos.Input
{
    public class StudentAttendanceDto
    {
        public required Guid StudentId { get; set; }
        public required bool IsPresent { get; set; }
        public string? Observation { get; set; }
    }
}
