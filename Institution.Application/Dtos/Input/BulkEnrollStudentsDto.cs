namespace Institution.Application.Dtos.Input
{
    public class BulkEnrollStudentsDto
    {
        public Guid ProgramEditionId { get; set; }
        public required List<BulkEnrollStudentItemDto> Students { get; set; }
    }

    public class BulkEnrollStudentItemDto
    {
        public required Guid StudentId { get; set; }
        public int? GradeOrYear { get; set; }
    }
}
