namespace Institution.Application.Dtos.Input
{
    public class CreateSubjectDto
    {
        public required Guid AcademicProgramId { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
    }
}
