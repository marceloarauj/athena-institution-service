using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListEventsHandler
    (
        IUnitOfWork unitOfWork,
        IInstitutionContext institutionContext
    ) : IMessageHandler<ListEventsCommand, AthenaApiResponse<List<EventResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<EventResponseDto>>> Handle(ListEventsCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);

            if (institution == null)
                return AthenaApiResponse<List<EventResponseDto>>.NotFound("Institution not found.");

            var filter = request.Filter;
            var now = DateTime.UtcNow;
            var startDate = filter.StartDate ?? new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var endDate = filter.EndDate ?? new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59, DateTimeKind.Utc);

            var events = await unitOfWork.EventRepository.GetByFilterAsync(institution.Id, filter.Name, startDate, endDate);

            var response = events.Select(@event => new EventResponseDto(@event)).ToList();

            return AthenaApiResponse<List<EventResponseDto>>.Ok(response);
        }
    }
}
