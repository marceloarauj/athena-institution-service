using AthenaUnionLibrary.ApiResponse;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record GenerateReportCardCommand : IRequestMessage<AthenaApiResponse<byte[]>>;
}
