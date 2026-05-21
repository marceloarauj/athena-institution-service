namespace Institution.Application.Dtos.Input
{
    public class GenerateClassGroupsDto
    {
        public required Guid ProgramEditionId { get; set; }
        public required List<ClassGroupConfigDto> Groups { get; set; }
    }

    public class ClassGroupConfigDto
    {
        public required int GradeOrYear { get; set; }
        public required int NumberOfGroups { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? ShiftId { get; set; }
        public required int MaxStudents { get; set; }
    }
}
