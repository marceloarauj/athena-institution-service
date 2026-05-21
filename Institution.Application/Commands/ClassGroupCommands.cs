using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record CreateClassGroupCommand(CreateClassGroupDto Dto) : IRequestMessage<AthenaApiResponse<ClassGroupResponseDto>>;
    public record GenerateClassGroupsCommand(GenerateClassGroupsDto Dto) : IRequestMessage<AthenaApiResponse<List<ClassGroupResponseDto>>>;
    public record AssignStudentsToGroupsCommand(AssignStudentsToGroupsDto Dto) : IRequestMessage<AthenaApiResponse<List<ClassGroupStudentResponseDto>>>;
    public record ListClassGroupsCommand(Guid ProgramEditionId) : IRequestMessage<AthenaApiResponse<List<ClassGroupResponseDto>>>;
    public record GetClassGroupCommand(Guid Id) : IRequestMessage<AthenaApiResponse<ClassGroupResponseDto>>;
}
