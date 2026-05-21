namespace Institution.Application.Dtos.Input
{
    public class CreateRecessDto
    {
        public required Guid ProgramEditionId { get; set; }
        public required string Name { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
    }
}
