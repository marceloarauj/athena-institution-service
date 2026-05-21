using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record GetPublishChecklistCommand(Guid ProgramEditionId) : IRequestMessage<AthenaApiResponse<PublishChecklistDto>>;
    public record PublishProgramEditionCommand(Guid ProgramEditionId) : IRequestMessage<AthenaApiResponse<ProgramEditionResponseDto>>;
    public record CloseProgramEditionCommand(Guid ProgramEditionId) : IRequestMessage<AthenaApiResponse<ProgramEditionResponseDto>>;
}
