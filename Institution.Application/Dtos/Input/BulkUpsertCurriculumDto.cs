namespace Institution.Application.Dtos.Input
{
    public class BulkUpsertCurriculumDto
    {
        public Guid ProgramEditionId { get; set; }
        public required List<UpsertCurriculumEntryDto> Entries { get; set; }
    }
}
