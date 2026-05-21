using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record GeneratePeriodsCommand(GeneratePeriodsDto Dto) : IRequestMessage<AthenaApiResponse<List<ProgramPeriodResponseDto>>>;
    public record CreatePeriodCommand(CreatePeriodDto Dto) : IRequestMessage<AthenaApiResponse<ProgramPeriodResponseDto>>;
    public record ListPeriodsCommand(Guid ProgramEditionId) : IRequestMessage<AthenaApiResponse<List<ProgramPeriodResponseDto>>>;
}
