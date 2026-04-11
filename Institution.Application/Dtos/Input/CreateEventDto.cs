using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Input
{
    public class CreateEventDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }

        public EventEntity ToEntity(InstitutionEntity institution)
        {
            return new EventEntity
            {
                Name = Name,
                Description = Description,
                StartDate = StartDate,
                EndDate = EndDate,
                Institution = institution,
                InstitutionId = institution.Id
            };
        }
    }
}
