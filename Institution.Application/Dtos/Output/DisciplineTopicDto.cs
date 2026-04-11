using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class DisciplineTopicDto(DisciplineTopicEntity topic)
    {
        public Guid Id { get; set; } = topic.Id;
        public string Content { get; set; } = topic.Content;
        public int LessonNumber { get; set; } = topic.LessonNumber;
    }
}
