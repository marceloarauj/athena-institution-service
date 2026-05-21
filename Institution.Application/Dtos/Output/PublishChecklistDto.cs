namespace Institution.Application.Dtos.Output
{
    public class PublishChecklistDto
    {
        public Guid EditionId { get; set; }
        public bool CanPublish { get; set; }
        public List<PublishCheckItemDto> Checks { get; set; } = [];
    }

    public class PublishCheckItemDto
    {
        public string Name { get; set; } = string.Empty;
        public bool Passed { get; set; }
        public string Detail { get; set; } = string.Empty;
    }
}
