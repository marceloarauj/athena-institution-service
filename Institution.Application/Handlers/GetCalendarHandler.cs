using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GetCalendarHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<GetCalendarCommand, AthenaApiResponse<List<CalendarDayResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<CalendarDayResponseDto>>> Handle(GetCalendarCommand request, CancellationToken cancellationToken)
        {
            var days = await unitOfWork.CalendarDayRepository.GetByEditionAsync(request.ProgramEditionId);
            return AthenaApiResponse<List<CalendarDayResponseDto>>.Ok(days.Select(d => new CalendarDayResponseDto(d)).ToList());
        }
    }
}
