namespace Institution.Application.Dtos.Input
{
    public class CreateDisciplineTopicDto
    {
        public required string Content { get; set; }
        public int LessonNumber { get; set; }
    }
}
