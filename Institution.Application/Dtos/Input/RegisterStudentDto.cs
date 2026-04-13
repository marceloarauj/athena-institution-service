namespace Institution.Application.Dtos.Input
{
    public class RegisterStudentDto
    {
        public required Guid StudentId { get; set; }
        public required Guid ClassroomId { get; set; }
    }
}
