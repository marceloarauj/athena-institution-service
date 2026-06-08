using AthenaUnionLibrary.ApiResponse;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record DeleteDisciplineCommand(Guid Id) : IRequestMessage<AthenaApiResponse<object>>;
}
