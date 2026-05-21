using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Input
{
    public class CreateHolidayDto
    {
        public required string Name { get; set; }
        public required DateOnly Date { get; set; }
        public required HolidayType Type { get; set; }
        public bool IsRecurring { get; set; }
    }
}
