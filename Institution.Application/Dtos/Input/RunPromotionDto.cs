namespace Institution.Application.Dtos.Input
{
    public class RunPromotionDto
    {
        public Guid SourceEditionId { get; set; }
        public required Guid TargetEditionId { get; set; }
    }
}
