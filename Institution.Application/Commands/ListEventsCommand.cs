using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record ListEventsCommand(ListEventsFilterDto Filter) : IRequestMessage<AthenaApiResponse<List<EventResponseDto>>>;
}
