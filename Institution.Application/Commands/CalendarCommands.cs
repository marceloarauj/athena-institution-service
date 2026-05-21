using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record GenerateCalendarCommand(GenerateCalendarDto Dto) : IRequestMessage<AthenaApiResponse<CalendarSummaryDto>>;
    public record GetCalendarCommand(Guid ProgramEditionId) : IRequestMessage<AthenaApiResponse<List<CalendarDayResponseDto>>>;
    public record GetCalendarSummaryCommand(Guid ProgramEditionId) : IRequestMessage<AthenaApiResponse<CalendarSummaryDto>>;
}
