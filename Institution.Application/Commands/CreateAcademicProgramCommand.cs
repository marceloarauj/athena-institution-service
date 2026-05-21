using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record CreateAcademicProgramCommand(CreateAcademicProgramDto Dto) : IRequestMessage<AthenaApiResponse<AcademicProgramResponseDto>>;
    public record ListAcademicProgramsCommand() : IRequestMessage<AthenaApiResponse<List<AcademicProgramResponseDto>>>;
    public record GetAcademicProgramCommand(Guid Id) : IRequestMessage<AthenaApiResponse<AcademicProgramResponseDto>>;
}
