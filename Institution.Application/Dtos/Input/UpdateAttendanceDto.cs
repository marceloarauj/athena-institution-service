namespace Institution.Application.Dtos.Input
{
    public class UpdateAttendanceDto
    {
        public required List<StudentAttendanceDto> Students { get; set; }
    }
}
