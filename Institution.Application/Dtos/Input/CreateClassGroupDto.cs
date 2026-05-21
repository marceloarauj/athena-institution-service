namespace Institution.Application.Dtos.Input
{
    public class CreateClassGroupDto
    {
        public Guid ProgramEditionId { get; set; }
        public required string Name { get; set; }
        public int? GradeOrYear { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? ShiftId { get; set; }
        public required int MaxStudents { get; set; }
    }
}
