using Institution.Domain.Entities;
using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Output
{
    public class HolidayResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly Date { get; set; }
        public HolidayType Type { get; set; }
        public bool IsRecurring { get; set; }

        public HolidayResponseDto(HolidayEntity e)
        {
            Id = e.Id;
            Name = e.Name;
            Date = e.Date;
            Type = e.Type;
            IsRecurring = e.IsRecurring;
        }
    }
}
