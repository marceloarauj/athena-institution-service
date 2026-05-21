namespace Institution.Application.Dtos.Input
{
    public class CreateProgramEditionDto
    {
        public required Guid AcademicProgramId { get; set; }
        public required string Name { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
    }
}
