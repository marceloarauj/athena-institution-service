using Institution.Domain.Entities;
using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Output
{
    public class CalendarDayResponseDto
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public CalendarDayType Type { get; set; }
        public string? HolidayName { get; set; }
        public Guid? ProgramPeriodId { get; set; }

        public CalendarDayResponseDto(CalendarDayEntity e)
        {
            Id = e.Id;
            Date = e.Date;
            Type = e.Type;
            HolidayName = e.HolidayName;
            ProgramPeriodId = e.ProgramPeriodId;
        }
    }
}
