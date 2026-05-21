using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Enums;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GetCalendarSummaryHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<GetCalendarSummaryCommand, AthenaApiResponse<CalendarSummaryDto>>
    {
        public async Task<AthenaApiResponse<CalendarSummaryDto>> Handle(GetCalendarSummaryCommand request, CancellationToken cancellationToken)
        {
            var days = await unitOfWork.CalendarDayRepository.GetByEditionAsync(request.ProgramEditionId);
            var periods = await unitOfWork.ProgramPeriodRepository.GetByEditionAsync(request.ProgramEditionId);

            int totalSchoolDays = days.Count(d => d.Type == CalendarDayType.SchoolDay);
            int totalHolidays = days.Count(d => d.Type == CalendarDayType.Holiday);
            int totalRecesses = days.Count(d => d.Type == CalendarDayType.Recess);

            var periodSummaries = periods.Select(p => new PeriodSummaryDto
            {
                PeriodId = p.Id,
                PeriodName = p.Name,
                SchoolDays = days.Count(d => d.Type == CalendarDayType.SchoolDay && d.ProgramPeriodId == p.Id)
            }).ToList();

            return AthenaApiResponse<CalendarSummaryDto>.Ok(new CalendarSummaryDto
            {
                TotalSchoolDays = totalSchoolDays,
                SchoolDaysByPeriod = periodSummaries,
                TotalHolidays = totalHolidays,
                TotalRecesses = totalRecesses
            });
        }
    }
}
