using Institution.Domain.Entities;

namespace Institution.Application.Dtos.Output
{
    public class EventResponseDto(EventEntity @event)
    {
        public Guid Id { get; set; } = @event.Id;
        public string Name { get; set; } = @event.Name;
        public string? Description { get; set; } = @event.Description;
        public DateTime StartDate { get; set; } = @event.StartDate;
        public DateTime EndDate { get; set; } = @event.EndDate;
    }
}
