namespace Institution.Application.Dtos.Output
{
    public class CalendarSummaryDto
    {
        public int TotalSchoolDays { get; set; }
        public List<PeriodSummaryDto> SchoolDaysByPeriod { get; set; } = [];
        public int TotalHolidays { get; set; }
        public int TotalRecesses { get; set; }
    }

    public class PeriodSummaryDto
    {
        public Guid PeriodId { get; set; }
        public string PeriodName { get; set; } = string.Empty;
        public int SchoolDays { get; set; }
    }
}
