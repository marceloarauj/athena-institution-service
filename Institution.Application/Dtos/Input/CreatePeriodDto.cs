namespace Institution.Application.Dtos.Input
{
    public class CreatePeriodDto
    {
        public Guid ProgramEditionId { get; set; }
        public required int Number { get; set; }
        public required string Name { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
    }
}
