using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record CreateProgramEditionCommand(CreateProgramEditionDto Dto) : IRequestMessage<AthenaApiResponse<ProgramEditionResponseDto>>;
    public record ListProgramEditionsCommand(Guid AcademicProgramId) : IRequestMessage<AthenaApiResponse<List<ProgramEditionResponseDto>>>;
    public record GetProgramEditionCommand(Guid Id) : IRequestMessage<AthenaApiResponse<ProgramEditionResponseDto>>;
}
