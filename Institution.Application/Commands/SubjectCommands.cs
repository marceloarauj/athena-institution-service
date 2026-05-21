using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record CreateSubjectCommand(CreateSubjectDto Dto) : IRequestMessage<AthenaApiResponse<SubjectResponseDto>>;
    public record ListSubjectsCommand(Guid AcademicProgramId) : IRequestMessage<AthenaApiResponse<List<SubjectResponseDto>>>;
}
