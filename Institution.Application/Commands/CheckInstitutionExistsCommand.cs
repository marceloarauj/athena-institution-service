using AthenaUnionLibrary.ApiResponse;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record CheckInstitutionExistsCommand(string Alias) : IRequestMessage<AthenaApiResponse<bool>>;
}
