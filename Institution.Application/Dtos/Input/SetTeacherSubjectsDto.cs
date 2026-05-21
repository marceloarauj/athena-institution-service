namespace Institution.Application.Dtos.Input
{
    public class SetTeacherSubjectsDto
    {
        public Guid TeacherId { get; set; }
        public required List<Guid> SubjectIds { get; set; }
    }
}
