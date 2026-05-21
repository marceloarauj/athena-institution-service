namespace Institution.Application.Dtos.Input
{
    public class CreateTeacherDto
    {
        public required Guid UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
