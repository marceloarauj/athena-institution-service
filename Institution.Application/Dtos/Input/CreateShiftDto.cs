namespace Institution.Application.Dtos.Input
{
    public class CreateShiftDto
    {
        public required string Name { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
    }
}
