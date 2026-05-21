using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GenerateCalendarHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<GenerateCalendarCommand, AthenaApiResponse<CalendarSummaryDto>>
    {
        public async Task<AthenaApiResponse<CalendarSummaryDto>> Handle(GenerateCalendarCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<CalendarSummaryDto>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.Dto.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<CalendarSummaryDto>.NotFound("Program edition not found.");

            if (IsEditionLocked(edition))
                return AthenaApiResponse<CalendarSummaryDto>.UnprocessableEntity("Cannot generate calendar for a published or closed edition.");

            var startYear = edition.StartDate.Year;
            var endYear = edition.EndDate.Year;

            var holidays = await unitOfWork.HolidayRepository.GetByYearAsync(institution.Id, startYear);
            if (startYear != endYear)
            {
                var extraHolidays = await unitOfWork.HolidayRepository.GetByYearAsync(institution.Id, endYear);
                holidays = holidays.Union(extraHolidays, EqualityComparer<HolidayEntity>.Create(
                    (a, b) => a!.Id == b!.Id, h => h!.Id.GetHashCode())).ToList();
            }

            var recesses = await unitOfWork.HolidayRepository.GetRecessesByEditionAsync(edition.Id);
            var periods = await unitOfWork.ProgramPeriodRepository.GetByEditionAsync(edition.Id);

            var holidayDates = new HashSet<DateOnly>(holidays.Select(h =>
                h.IsRecurring ? new DateOnly(edition.StartDate.Year, h.Date.Month, h.Date.Day) : h.Date));

            // Also add non-recurring holidays for the end year if range spans years
            foreach (var h in holidays.Where(h => !h.IsRecurring))
                holidayDates.Add(h.Date);

            var recessDates = new HashSet<DateOnly>();
            foreach (var recess in recesses)
            {
                var d = recess.StartDate;
                while (d <= recess.EndDate)
                {
                    recessDates.Add(d);
                    d = d.AddDays(1);
                }
            }

            var calendarDays = new List<CalendarDayEntity>();
            var current = edition.StartDate;

            while (current <= edition.EndDate)
            {
                var type = CalendarDayType.SchoolDay;
                string? holidayName = null;
                Guid? periodId = null;

                var isWeekend = current.DayOfWeek == DayOfWeek.Saturday || current.DayOfWeek == DayOfWeek.Sunday;
                var matchingPeriod = periods.FirstOrDefault(p => current >= p.StartDate && current <= p.EndDate);
                periodId = matchingPeriod?.Id;

                if (isWeekend && !edition.AcademicProgram.HasWeeklySchedule)
                {
                    type = CalendarDayType.Weekend;
                }
                else if (recessDates.Contains(current))
                {
                    type = CalendarDayType.Recess;
                }
                else if (holidayDates.Contains(current))
                {
                    type = CalendarDayType.Holiday;
                    // Find recurring holiday name
                    var holiday = holidays.FirstOrDefault(h =>
                        (h.IsRecurring && h.Date.Month == current.Month && h.Date.Day == current.Day) ||
                        (!h.IsRecurring && h.Date == current));
                    holidayName = holiday?.Name;
                }
                else if (matchingPeriod == null)
                {
                    type = CalendarDayType.PeriodBreak;
                }

                calendarDays.Add(new CalendarDayEntity
                {
                    Date = current,
                    Type = type,
                    HolidayName = holidayName,
                    ProgramEditionId = edition.Id,
                    ProgramEdition = edition,
                    ProgramPeriodId = periodId,
                    ProgramPeriod = matchingPeriod
                });

                current = current.AddDays(1);
            }

            // Count school days per period and update
            var periodSummaries = new List<PeriodSummaryDto>();
            foreach (var period in periods)
            {
                int schoolDays = calendarDays.Count(d => d.Type == CalendarDayType.SchoolDay && d.ProgramPeriodId == period.Id);
                period.SchoolDays = schoolDays;
                await unitOfWork.ProgramPeriodRepository.UpdateSchoolDaysAsync(period);

                periodSummaries.Add(new PeriodSummaryDto
                {
                    PeriodId = period.Id,
                    PeriodName = period.Name,
                    SchoolDays = schoolDays
                });
            }

            int totalSchoolDays = calendarDays.Count(d => d.Type == CalendarDayType.SchoolDay);
            int totalHolidays = calendarDays.Count(d => d.Type == CalendarDayType.Holiday);
            int totalRecesses = calendarDays.Count(d => d.Type == CalendarDayType.Recess);

            var program = edition.AcademicProgram;
            if (program.MinSchoolDays.HasValue && totalSchoolDays < program.MinSchoolDays.Value)
                return AthenaApiResponse<CalendarSummaryDto>.UnprocessableEntity(
                    $"Calendar has {totalSchoolDays} school days, but minimum required is {program.MinSchoolDays.Value}.");

            await unitOfWork.CalendarDayRepository.DeleteByEditionAsync(edition.Id);
            await unitOfWork.CalendarDayRepository.AddRangeAsync(calendarDays);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<CalendarSummaryDto>.Ok(new CalendarSummaryDto
            {
                TotalSchoolDays = totalSchoolDays,
                SchoolDaysByPeriod = periodSummaries,
                TotalHolidays = totalHolidays,
                TotalRecesses = totalRecesses
            });
        }

        private static bool IsEditionLocked(ProgramEditionEntity edition) =>
            edition.Status == EditionStatus.Published || edition.Status == EditionStatus.Closed;
    }
}
