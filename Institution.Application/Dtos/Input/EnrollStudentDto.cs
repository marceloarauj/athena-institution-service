namespace Institution.Application.Dtos.Input
{
    public class EnrollStudentDto
    {
        public Guid ProgramEditionId { get; set; }
        public required Guid StudentId { get; set; }
        public int? GradeOrYear { get; set; }
        public string? PurchaseReference { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
